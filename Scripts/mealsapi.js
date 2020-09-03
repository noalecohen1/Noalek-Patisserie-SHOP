$(document).ready(function () {
    $('#meal-ideas').on('submit', function (e) {
        e.preventDefault();
        var requestData = $('#search').val();
        var result = $('#result-div');
        $.ajax({
            url: 'https://www.themealdb.com/api/json/v1/1/search.php?s=' + requestData,
            method: 'get',
            success: function (data) {
                for (let meal of data.meals) {
                    result.append(`<span class="p-2">${meal.strMeal}</span>`);
                }
            }
        });
    });
});