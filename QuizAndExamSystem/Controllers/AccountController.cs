using ExamSystem.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ExamSystem.Data.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using ExamSystem.Data.Static;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authentication.Cookies;
using ExamSystem.Filters;
using ExamSystem.Models;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text;
using ExamSystem.Data.Interface;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ExamSystem.Controllers
{
    [BreadcrumbActionFilter]
    public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IEmailService _emailservice;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;    // for including WC constant paths

        //constructor
        public AccountController(ILogger<AccountController> logger, IEmailService emailservice, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _logger = logger;
            _emailservice = emailservice;
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        // list all registered users to admin 
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AllUsers()
        {

            var user = await _userManager.Users.ToListAsync();
            if (user == null)
            {
                return NotFound();
            }
            _logger.LogInformation("All User page of Account Contorller is accessed by {0}", User.Identity.Name);
            return View(user);
        }

        public async Task<IActionResult> AllProfiles()
        {
            var obj = await _context.SchoolInfos.ToListAsync();
            if (obj == null)
            {
                return NotFound();
            }
            _logger.LogInformation("All User profiles page of Account Contorller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }

        // get user profile 
        // GET: account/user/Profile/1
        [Authorize]
        public async Task<IActionResult> Profile(string id)
        {
            var userdata = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            // Get the role names for the user
            if (userdata == null) return NotFound();
            var userRoles = await _userManager.GetRolesAsync(userdata);
            string role = userRoles.FirstOrDefault();
            var userClaims = await _userManager.GetClaimsAsync(userdata);


            var profile = new ProfileVM()
            {
                Id = userdata.Id,
                Role = role,
                Permissions = userClaims.Select(c => $"{c.Type}: {c.Value}").ToList(),
                IsEmailConfirmed = userdata.EmailConfirmed,
                Status = userdata.Status,
                //profile information
                FirstName = userdata.FirstName,
                LastName = userdata.LastName,
                FatherName = userdata.FatherName,
                DateOfBirth = userdata.DateOfBirth,
                Gender = userdata.Gender,
                Email = userdata.Email,
                UserName = userdata.Email,
                Biography = userdata.Biography,
                CNIC = userdata.CNIC,
                ProfileImage = userdata.ProfileImage,
                ContactNo = userdata.ContactNo,
                CellPhoneNo = userdata.CellPhoneNo,
                WhatsApp = userdata.WhatsApp,
                HomeAddress = userdata.HomeAddress,
                City = userdata.City,
                PostalCode = userdata.PostalCode,
                State = userdata.State,
                Country = userdata.Country,
                HighestDegree = userdata.HighestDegree,
                DegreeName = userdata.DegreeName,
                Facebook = userdata.Facebook,
                Twitter = userdata.Twitter,
                Linkedin = userdata.Linkedin,
                Instagram = userdata.Instagram,
                Pinterest = userdata.Pinterest,
            };
            if (profile == null)
            {
                TempData["error"] = "UserDetial is empty";
                return RedirectToAction(nameof(NotFound));
            }
            _logger.LogInformation("User profile page of Account Contorller is accessed by {0}", User.Identity.Name);
            return View(profile);
        }

        // edit user profile
        //GET: account/user/EditProfile/1
        [Authorize()]
        public async Task<IActionResult> EditProfile(string id)
        {
            var userdata = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            // Get the role names for the user
            var roleNames = await _userManager.GetRolesAsync(userdata);
            string? role = roleNames.FirstOrDefault();  // Now 'roles' is a list of role names the user belongs to
            var userClaims = await _userManager.GetClaimsAsync(userdata);

            var profile = new ProfileVM()
            {
                Id = userdata.Id,
                Role = role,
                Permissions = userClaims.Select(c => $"{c.Type}: {c.Value}").ToList(),
                //profile information
                FirstName = userdata.FirstName,
                LastName = userdata.LastName,
                FatherName = userdata.FatherName,
                DateOfBirth = userdata.DateOfBirth,
                Email = userdata.Email,
                UserName = userdata.Email,
                Biography = userdata.Biography,
                CNIC = userdata.CNIC,
                ProfileImage = userdata.ProfileImage,
                ContactNo = userdata.ContactNo,
                CellPhoneNo = userdata.CellPhoneNo,
                WhatsApp = userdata.WhatsApp,
                HomeAddress = userdata.HomeAddress,
                City = userdata.City,
                PostalCode = userdata.PostalCode,
                State = userdata.State,
                Country = userdata.Country,
                HighestDegree = userdata.HighestDegree,
                DegreeName = userdata.DegreeName,
                Facebook = userdata.Facebook,
                Twitter = userdata.Twitter,
                Linkedin = userdata.Linkedin,
                Instagram = userdata.Instagram,
                Pinterest = userdata.Pinterest,
            };
            if (profile == null)
            {
                TempData["error"] = "UserDetial is empty";
                return RedirectToAction(nameof(NotFound));
            }
            _logger.LogInformation("Edit profile page of Account Contorller is accessed by {0}", User.Identity.Name);
            return View(profile);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditProfile(string id, ProfileVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model is not valid";
                return View(nameof(NotFound));
            }
            var oldProfileData = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                // check weather the profile image is updated or uploaded
                string webRootPath = _webHostEnvironment.WebRootPath;
                string profileUpload = webRootPath + WC.ProfileImagePath;
                string fileName = User.Identity.Name + Guid.NewGuid().ToString().Substring(0, 5);
                string extension = Path.GetExtension(files[0].FileName);

                //delete the old file
                var oldfile = Path.Combine(profileUpload, oldProfileData.ProfileImage);
                //check if the file already exists then delete the old file
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);

                }

                // copy the file in system folder
                using (var filePicstream = new FileStream(Path.Combine(profileUpload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(filePicstream);
                }
                model.ProfileImage = fileName + extension;
            }
            else
            {
                model.ProfileImage = oldProfileData.ProfileImage;
            }
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            if (user != null)
            {
                //profile data bind
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.ProfileImage = model.ProfileImage;
                user.DateOfBirth = model.DateOfBirth;
                user.Gender = model.Gender;
                user.Biography = model.Biography;
                user.FatherName = model.FatherName;
                user.CNIC = model.CNIC;
                user.ContactNo = model.ContactNo;
                user.CellPhoneNo = model.CellPhoneNo;
                user.WhatsApp = model.WhatsApp;
                user.HomeAddress = model.HomeAddress;
                user.PostalCode = model.PostalCode;
                user.City = model.City;
                user.Country = model.Country;
                user.HighestDegree = model.HighestDegree;
                user.DegreeName = model.DegreeName;
                user.Facebook = model.Facebook;
                user.Twitter = model.Twitter;
                user.Linkedin = model.Linkedin;
                user.Instagram = model.Instagram;
                user.Pinterest = model.Pinterest;

                //commet changes to database
                await _userManager.UpdateAsync(user);
            }
            //check the selected role and assign to it
            // update the permissions accoriding to user roles
            // Update or set permissions for the user based on the selected role

            var roleNames = await _userManager.GetRolesAsync(user);
            var existingRole = roleNames.FirstOrDefault();
            if (string.IsNullOrEmpty(existingRole))
            {
                string role = model.Role;
                if (role == UserRoles.Teacher)
                {
                    // add the permissions 
                    await _userManager.AddToRoleAsync(user, UserRoles.Teacher);
                    await _userManager.AddClaimAsync(user, new Claim("Permission", UserPermissions.Reader));
                    _logger.LogInformation("Teacher role is assigned in profile to {0}", user.UserName);
                }
                else if (role == UserRoles.Student)
                {
                    await _userManager.AddToRoleAsync(user, UserRoles.Student);
                    await _userManager.AddClaimAsync(user, new Claim("Permission", UserPermissions.Reader));
                    _logger.LogInformation("Student role is assigned in profile to {0}", user.UserName);
                }
                // Commit changes to the database
                await _userManager.UpdateAsync(user);
            }
            else
            {
                // Update permissions based on role change
                // For example, remove old permissions and add new permissions

                // Remove existing claims (permissions) associated with the old role
                var existingClaims = await _userManager.GetClaimsAsync(user);
                foreach (var claim in existingClaims)
                {
                    // Check and remove claims based on the old role's permissions
                    if (claim.Type == "Permission") /* &&  condition to check old role's permission )*/
                    {
                        await _userManager.RemoveClaimAsync(user, claim);
                    }
                }

                string role = model.Role;

                if (role == UserRoles.Teacher)
                {
                    // Add new permissions based on the selected permissions in the ViewModel
                    foreach (var selectedPermission in model.Permissions)
                    {
                        await _userManager.AddClaimAsync(user, new Claim("Permission", selectedPermission));
                        _logger.LogInformation("Permission '{0}' assigned to {1}", selectedPermission, user.UserName);
                    }
                }
                else if (role == UserRoles.Student)
                {
                    // Add new permissions based on the selected permissions in the ViewModel
                    foreach (var selectedPermission in model.Permissions)
                    {
                        await _userManager.AddClaimAsync(user, new Claim("Permission", selectedPermission));
                        _logger.LogInformation("Permission '{0}' assigned to {1}", selectedPermission, user.UserName);
                    }
                }

                // ... other updates to user data ...

                // Commit changes to the database
                await _userManager.UpdateAsync(user);
            }

            _logger.LogInformation("Profile is successfully modified by {0}", User.Identity.Name);
            return RedirectToAction("Profile", new { @id = model.Id });
        }


        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> ConfirmEmailByAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            if (!user.EmailConfirmed)
            {
                user.EmailConfirmed = true;

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {
                    // Email confirmed successfully
                    return RedirectToAction(nameof(Profile), new { @id = user.Id });
                }
                else
                {
                    // Handle error while confirming email
                    ModelState.AddModelError(string.Empty, "Error confirming email.");
                    return RedirectToAction(nameof(Profile), new { @id = user.Id });
                }
            }
            else
            {
                // Email is already confirmed

                return RedirectToAction(nameof(Profile), new { @id = user.Id });
            }
        }


        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> ChangeUserStatusByAdmin(string id, Status status)
        {
            var user = await _userManager.FindByIdAsync(id);
          
            if (user == null)
            {
                return NotFound();
            }

            user.Status = status;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                // Profile status updated successfully
                return RedirectToAction(nameof(Profile), new { @id = user.Id });
            }
            else
            {
                // Handle error while updating profile status
                ModelState.AddModelError(string.Empty, "Error Chnging status of profile.");
                return RedirectToAction(nameof(Profile), new { @id = user.Id });
            }
        }



        //disply School Porfile
        public async Task<IActionResult> SchoolProfile(string id)
        {
            var schooldata = await _context.SchoolInfos.FirstOrDefaultAsync(u => u.Id == id);
            _logger.LogInformation("School Profile page is accessed by {0}", User.Identity.Name);
            if (schooldata == null)
            {
                return View(new SchoolVM());
            }
            else
            {
                var obj = new SchoolVM()
                {
                    Id = id,
                    //school information
                    SchoolType = schooldata.SchoolType,
                    SchoolName = schooldata.SchoolName,
                    SchoolCode = schooldata.SchoolCode,
                    SchoolAddress = schooldata.SchoolAddress,
                    SchoolContactNumber = schooldata.SchoolContactNumber,
                    SchoolDescription = schooldata.SchoolDescription,
                    SchoolEmail = schooldata.SchoolEmail,
                    SchoolLogo = schooldata.SchoolLogo,
                };
                return View(obj);
            }
        }

        // edit School profile
        //GET: account/user/EditSchoolProfile/1
        [HttpGet]
        public async Task<IActionResult> EditSchoolProfile(string id)
        {
            var schooldata = await _context.SchoolInfos.FirstOrDefaultAsync(u => u.Id == id);
            var obj = new SchoolVM()
            {
                Id = id,
                //school information
                SchoolType = schooldata.SchoolType,
                SchoolName = schooldata.SchoolName,
                SchoolCode = schooldata.SchoolCode,
                SchoolAddress = schooldata.SchoolAddress,
                SchoolContactNumber = schooldata.SchoolContactNumber,
                SchoolDescription = schooldata.SchoolDescription,
                SchoolEmail = schooldata.SchoolEmail,
                SchoolLogo = schooldata.SchoolEmail,
            };
            if (obj == null)
            {
                TempData["error"] = "No School profile found";
                return RedirectToAction(nameof(NotFound));
            }
            _logger.LogInformation("Edit School Profile page of Account Contorller is accessed by {0}", User.Identity.Name);
            return View(obj);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> EditSchoolProfile(string id, SchoolVM model)
        {
            if (!ModelState.IsValid)
            {
                TempData["error"] = "Model is not valid";
                return View(nameof(NotFound));
            }
            var oldSchoolData = await _context.SchoolInfos.FindAsync(id);
            // check weather the profile image is updated or uploaded
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                string webRootPath = _webHostEnvironment.WebRootPath;
                string logoUpload = webRootPath + WC.ProfileImagePath;
                string fileName = User.Identity.Name + Guid.NewGuid().ToString().Substring(0, 5);
                string extension = Path.GetExtension(files[0].FileName);

                //delete the old file
                var oldfile = Path.Combine(logoUpload, oldSchoolData.SchoolLogo);
                //check if the file already exists then delete the old file
                if (System.IO.File.Exists(oldfile))
                {
                    System.IO.File.Delete(oldfile);

                }
                // copy the file in system folder
                using (var filePicstream = new FileStream(Path.Combine(logoUpload, fileName + extension), FileMode.Create))
                {
                    files[0].CopyTo(filePicstream);
                }
                model.SchoolLogo = fileName + extension;
            }
            else
            {
                model.SchoolLogo = oldSchoolData.SchoolLogo;
            }
            //create schoolinfo object
            var schoolInfo = await _context.SchoolInfos.FindAsync(id);
            if (schoolInfo != null)
            {
                // school profile data bind
                schoolInfo.SchoolName = model.SchoolName;
                schoolInfo.SchoolEmail = model.SchoolEmail;
                schoolInfo.SchoolAddress = model.SchoolAddress;
                schoolInfo.SchoolCode = model.SchoolCode;
                schoolInfo.SchoolContactNumber = model.SchoolContactNumber;
                schoolInfo.SchoolType = model.SchoolType;
                if (model.SchoolLogo != null)
                {
                    schoolInfo.SchoolLogo = model.SchoolLogo;
                }

                //commet the changes to database
                await _context.SaveChangesAsync();
                _logger.LogInformation("School Profile is updated/created by {0}", User.Identity.Name);
            }
            return RedirectToAction("SchoolProfile", new { @id = model.Id });
        }


        #region Register

        public IActionResult Register()
        {
            // pass the view model to the view
            _logger.LogInformation("Register page of Account Contorller is accessed by {0}", User.Identity.Name);
            return View(new RegisterVM());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            //if model state is not valid return the view again to user
            if (!ModelState.IsValid) return View(registerVM);

            //if user model is valid then check the existance of user in database
            var user = await _userManager.FindByEmailAsync(registerVM.Email);
            if (user != null)
            {
                TempData["error"] = "Email already registered. Please try another email address";
                return RedirectToAction(nameof(Login));
            }

            //extract username from email address adn set it
            string? s = registerVM.Email;
            string userIdFromEmail = s.Substring(0, s.IndexOf('@'));
            string? returnUrl = Url.Content("~/");

            // register new user 
            var newUser = new ApplicationUser()
            {
                FirstName = registerVM.FirstName,
                LastName = registerVM.LastName,

                UserName = userIdFromEmail,
                Email = registerVM.Email
            };
            var newUserResponce = await _userManager.CreateAsync(newUser, registerVM.Password);

            // if new user successfully regsiter then assign a role and redirect to profile section
            if (newUserResponce.Succeeded)
            {
                _logger.LogInformation("New user created with new email address and password");
                await ConfirmEmailSend(registerVM, returnUrl, newUser);
                //check the selected role and assign to it
                var role = registerVM.Role;
                if (role == UserRoles.Teacher)
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Teacher);
                    _logger.LogInformation("Role Teacher is assigned to {0}", User.Identity.Name);
                    return RedirectToAction("Login", "Account");
                }
                else if (role == UserRoles.Student)
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Student);
                    _logger.LogInformation("Role Student is assigned to {0}", User.Identity.Name);
                    return RedirectToAction("Login", "Account");
                }
            }
            TempData["error"] = "Something bad happen.... Please try again after some time ";
            return View(registerVM);
        }

        [HttpGet]
        public async Task ConfirmEmailSend(RegisterVM registerVM, string? returnUrl, ApplicationUser? newUser)
        {
            var user = new ApplicationUser();
            user.Email = newUser == null ? registerVM.Email : newUser.Email;
            string returnAddress;
            returnAddress = returnUrl == null ? returnAddress = "~/" : returnAddress = returnUrl;


            //send an activation link to the newly registered user
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(System.Text.Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.ActionLink(
                action: "ConfirmEmail",
                controller: "Account",
                values: new { userId = newUser.Id, returnUrl = returnAddress },
                protocol: Request.Scheme
                );


            _logger.LogInformation("email with confirmation link is send to registered user");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{userId}'.");
            }

            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (!result.Succeeded)
            {
                _logger.LogInformation("Error confirming email for user with ID {0}:", userId);
                throw new InvalidOperationException($"Error confirming email for user with ID '{userId}':");
            }

            return View("ConfirmEmail");
        }

        #endregion


        #region Login


        public async Task<IActionResult> LoginAsync(string? returnUrl = null)
        {
            _logger.LogInformation("login function called");
            LoginVM model = new LoginVM()
            {
                ReturnUrl = Url.Content("~/"),
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };
            _logger.LogInformation("Login page of Account Contorller is accessed ");
            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            //if model state is not valid return the view again to user
            if (!ModelState.IsValid)
            {
                var validationErrors = ModelState.Values.Where(E => E.Errors.Count > 0)
                    .SelectMany(E => E.Errors)
                    .Select(E => E.ErrorMessage)
                    .ToList();
                TempData["error"] = "Invalid model state... Plz try again";
                return View(loginVM);
            }

            //if user model is valid then check the existance of user in database
            var user = await _userManager.FindByEmailAsync(loginVM.Email);
            //var normalizedEmail = _userManager.NormalizeEmail(loginVM.Email);
            //var user = await _userManager.Users.FirstOrDefaultAsync(x => x.NormalizedEmail == normalizedEmail);
            if (user != null)
            {
                //check if user email is not confirmed
                if (_userManager.Options.SignIn.RequireConfirmedAccount && !await _userManager.IsEmailConfirmedAsync(user))
                {
                    TempData["error"] = "Email address is not confirmed yet. Please confirm your emailaddress and try again";
                    return View(loginVM);
                }
                //check the password
                var passwordcheck = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passwordcheck)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.RememberMe, false);
                    _logger.LogInformation("User logged in successfully");
                    if (result.Succeeded)
                    {
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, loginVM.Email),
                            new Claim(ClaimTypes.Name, loginVM.Email)

                        };
                        var AccountUser = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);  //ClaimAccount(claims, CookieAuthenticationDefaults.AuthenticationScheme); //CookieAuthenticationDefaults.AuthenticationScheme);
                        var principal = new ClaimsPrincipal(AccountUser);

                        return RedirectToAction("Index", "Home");
                        //if (!string.IsNullOrEmpty(loginVM.ReturnUrl) && Url.IsLocalUrl(loginVM.ReturnUrl))
                        //{
                        //    return Redirect(loginVM.ReturnUrl);
                        //}
                        //else
                        //{
                        //    return RedirectToAction("Index", "Home");
                        //}

                    }
                    if (result.RequiresTwoFactor)
                    {
                        return RedirectToAction("LoginWith2FA", "Account");
                    }
                    if (result.IsLockedOut)
                    {
                        _logger.LogWarning("User account has been locked out");
                        return RedirectToAction("UserLocked", "Account");
                    }
                }
                // if password are not correct   
                TempData["error"] = "Wrong Credentials!, Please try again";
                return View(loginVM);
            }
            TempData["error"] = "Wrong Credentials!, Please try again";
            return View(loginVM);

        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult ExternalLogin(string provider, string returnUrl)
        {
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account",
                                    new { ReturnUrl = returnUrl });

            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            _logger.LogInformation("External Login page of Account Contorller is accessed by");

            return new ChallengeResult(provider, properties);
        }

        // login call back of external links like google and facebook etc
        [AllowAnonymous]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {


            LoginVM loginVM = new LoginVM
            {
                ReturnUrl = returnUrl,
                ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList()
            };

            if (remoteError != null)
            {
                ModelState
                    .AddModelError(string.Empty, $"Error from external provider: {remoteError}");

                return View("LoginUser", loginVM);
            }

            // Get the login information about the user from the external login provider
            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState
                    .AddModelError(string.Empty, "Error loading external login information.");

                return View("LoginUser", loginVM);
            }

            // If the user already has a login (i.e if there is a record in AspNetUserLogins
            // table) then sign-in the user with this external login provider
            var signInResult = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider,
                info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded)
            {
                return LocalRedirect(returnUrl);
            }
            // If there is no record in AspNetUserLogins table, the user may not have
            // a local account
            else
            {
                // Get the email claim value
                var email = info.Principal.FindFirstValue(ClaimTypes.Email);

                if (email != null)
                {
                    // Create a new user without password if we do not have a user already
                    var user = await _userManager.FindByEmailAsync(email);

                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = info.Principal.FindFirstValue(ClaimTypes.Email),
                            Email = info.Principal.FindFirstValue(ClaimTypes.Email)
                        };

                        await _userManager.CreateAsync(user);
                    }

                    // Add a login (i.e insert a row for the user in AspNetUserLogins table)
                    await _userManager.AddLoginAsync(user, info);
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    _logger.LogInformation("External Login attempt of Account Contorller is  by {0}", User.Identity.Name);
                    returnUrl = Url.Content("~/Account/EditProfile/" + user.Id);

                    return LocalRedirect(returnUrl);
                }

                // If we cannot find the user email we cannot continue
                ViewBag.ErrorTitle = $"Email claim not received from: {info.LoginProvider}";

                return View("Error");
            }
        }


        public IActionResult UserLocked()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Loginwith2FA()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> LoginWith2FA(LoginWith2FAVM model, bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Unable to load two-factor authentication user.");
            }

            var authenticatorCode = model.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, model.RememberMachine);

            if (result.Succeeded)
            {
                _logger.LogInformation("User with ID '{UserId}' logged in with 2fa.", user.Id);
                return LocalRedirect(returnUrl ?? Url.Content("~/"));
            }
            else if (result.IsLockedOut)
            {
                _logger.LogWarning("User with ID '{UserId}' account locked out.", user.Id);
                return RedirectToAction(nameof(UserLocked));
            }
            else
            {
                _logger.LogWarning("Invalid authenticator code entered for user with ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "Invalid authenticator code.");
                return View();
            }
        }


        ///Login section end here


        #endregion


        #region Forget Password adn Recovery
        //account/forgetpassword
        [AllowAnonymous]
        public IActionResult ForgotPassword()
        {
            _logger.LogInformation("Forget page of Account Contorller is accessed by {0}", User.Identity.Name);

            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ForgotPassword([Required] string email)
        {
            if (!ModelState.IsValid)
                return View(email);

            var user = await _userManager.FindByEmailAsync(email);
            if (user != null)
            {
                bool _hasUserPassword = await _userManager.HasPasswordAsync(user);
                if (!_hasUserPassword)
                {
                    TempData["error"] = "You are using this website with external account. Please try to update your password using the relavant website.";
                    return View();
                }

                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var link = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

                try
                {
                    await _emailservice.SendEmailAsync(user.Email, "ResetPassword", link);
                    _logger.LogInformation("forget page of Account Contorller is accessed by {0}", User.Identity.Name);
                    return RedirectToAction("ForgotPasswordConfirmation");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Failed to send reset password email.");
                    TempData["error"] = "Something gone wrong! reset email not sent";
                    return View();
                }
            }
            return View();
        }

        [AllowAnonymous]
        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }


        [AllowAnonymous]
        public IActionResult ResetPassword(string token, string email)
        {
            var model = new ResetPasswordVM { Token = token, Email = email };
            _logger.LogInformation("reset Password page of Account Contorller is accessed by {0}", User.Identity.Name);

            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM resetPassword)
        {
            if (!ModelState.IsValid)
                return View(resetPassword);

            var user = await _userManager.FindByEmailAsync(resetPassword.Email);
            if (user == null)
                RedirectToAction("ResetPasswordConfirmation");

            var resetPassResult = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.Password);
            if (!resetPassResult.Succeeded)
            {
                foreach (var error in resetPassResult.Errors)
                    ModelState.AddModelError(error.Code, error.Description);
                _logger.LogInformation("Password is reset by {0}", User.Identity.Name);
                return View();
            }

            return RedirectToAction("ResetPasswordConfirmation");
        }

        public IActionResult ResetPasswordConfirmation()
        {
            return View();
        }
        #endregion



        #region Logout
        // Signout method
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            _logger.LogInformation("Logout page of account controller is accessed");
            _logger.LogInformation("User is logged out successfully");

            return RedirectToAction("Index", "Home");
        }
        #endregion

    }
}
