$(document).ready(function () {

    $('#shop-button').on('click', function () {
        $.ajax({
            url: '/Game/Shop',
            type: 'GET',
            success: function (result) {
                $('#active-panel').html(result);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    $('#inventory-button').on('click', function () {
        $.ajax({
            url: '/Game/Inventory',
            type: 'GET',
            success: function (result) {
                $('#active-panel').html(result);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    function loadPlayerStats() {
        $('#stats').load('/Game/Stats');  
    }

    loadPlayerStats();
});