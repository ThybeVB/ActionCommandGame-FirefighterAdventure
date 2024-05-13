$(document).ready(function () {

    function loadPlayerStats() {
        let playerId = 1; //todo
        if (playerId) {
            $('#stats').load('/Game/ShowStats');
        }
    }

    loadPlayerStats();
});