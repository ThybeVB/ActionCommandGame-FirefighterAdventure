﻿$(document).ready(function () {

    $('#patrol-button').on('click', function () {
        $.ajax({
            url: '/Game/PerformAction',
            type: 'GET',
            success: function (result) {
                $('#active-panel').html(result);
                loadPlayerStats();
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

    $('#leaderboard-button').on('click', function () {
        $.ajax({
            url: '/Game/Leaderboard',
            type: 'GET',
            success: function (result) {
                $('#active-panel').html(result);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    $(document).on('click', '.buy-button', function () {
        var itemId = $(this).data('itemid');

        $.ajax({
            url: '/Game/BuyItem',
            type: 'GET',
            data: { itemId: itemId },
            success: function (response) {
                $('#active-panel').html(response);
                loadPlayerStats();
            },
            error: function (xhr, status, error) {
                console.error('An error occurred:', error);
                $('#active-panel').html('Error buying item.');
            }
        });
    });

    $('#loadModalButton').on('click', function () {
        var itemId = $(this).data('itemid');

        $.ajax({
            url: '/Item/Details',
            type: 'GET',
            data: { itemId: itemId },
            success: function (result) {
                $('#active-modal').html(result);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    });

    function loadPlayerStats() {
        $.ajax({
            url: '/Game/Stats',
            type: 'GET',
            success: function (result) {
                $('#stats').html(result);
            },
            error: function (xhr, status, error) {
                console.error(error);
            }
        });
    }

    loadPlayerStats();
});