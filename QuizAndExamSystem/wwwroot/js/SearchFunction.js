

$(document).ready(function () {
    $(".filter-button").click(function () {
        var value = $(this).attr('data-filter');
        if (value == "all") {
            $('.filter').show('1000');
        }
        else {
            $('.filter').not('.' + value).hide('3000');
            $('.filter').filter('.' + value).show('3000');
        }
    });
});

// search code for search bar    ok
$(".search").on("keyup", function () {
    var input = $(this).val().toUpperCase();

    $(".card").each(function () {
        if ($(this).data("string").toUpperCase().indexOf(input) < 0) {
            $(this).hide();
        } else {
            $(this).show();
        }

    })
});