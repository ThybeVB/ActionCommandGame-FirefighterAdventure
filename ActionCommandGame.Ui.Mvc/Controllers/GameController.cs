﻿using ActionCommandGame.Sdk;
using ActionCommandGame.Ui.Mvc.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ActionCommandGame.Ui.Mvc.Controllers
{
    [Authorize]
    public class GameController : Controller
    {
        private GameView? _view;

        private readonly PlayerSdk _playerSdk;
        private readonly ItemSdk _itemSdk;
        private readonly PlayerItemSdk _playerItemSdk;


        public GameController(PlayerSdk playerSdk, ItemSdk itemSdk, PlayerItemSdk playerItemSdk)
        {
            _playerSdk = playerSdk;
            _itemSdk = itemSdk;
            _playerItemSdk = playerItemSdk;
        }

        public async Task<IActionResult> Index()
        {
            //_playerId = int.Parse(User.FindFirstValue("Id"));
            //Console.WriteLine(User.FindFirstValue("Id"));
            //var player = await _playerSdk.Get(_playerId);
            //var items = await _itemSdk.Find();
            //
            //_view = new GameView
            //{
            //    Player = player,
            //    Items = items
            //};
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var user = await _playerSdk.Get(uId.Value);
            ViewData["PlayerName"] = user.Name;


            return View();
        }

        public async Task<IActionResult> PerformAction()
        {
            return View();
        }

        public async Task<IActionResult> Shop()
        {
            var allItems = await _itemSdk.Find();

            return PartialView("_ShopPartial", allItems);
        }

        public async Task<IActionResult> Inventory()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var ownedItems = await _playerItemSdk.Find(uId.Value);
            
            return PartialView("_InventoryPartial", ownedItems);

        }

        public async Task<IActionResult> Stats()
        {
            var uId = User.Claims.FirstOrDefault(c => c.Type == "Id");
            var result = await _playerSdk.Get(uId.Value);

            return PartialView("_StatsPartial", result);
        }

    }
}
