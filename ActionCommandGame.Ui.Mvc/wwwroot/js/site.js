$(document).ready(function () {

    $('#patrol-button').on('click', function () {
        $.ajax({
            url: '/Game/PerformAction',
            type: 'GET',
            success: function (result) {
                $('#active-panel').html(result);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

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