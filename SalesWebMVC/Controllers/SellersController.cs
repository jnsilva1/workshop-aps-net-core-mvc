﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMVC.Models;
using SalesWebMVC.Models.ViewModels;
using SalesWebMVC.Services;
using SalesWebMVC.Services.Exceptions;

namespace SalesWebMVC.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService;
        private readonly DepartamentService _departamentService;

        public SellersController(SellerService sellerService, DepartamentService departamentService)
        {
            _sellerService = sellerService;
            _departamentService = departamentService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll();
            return View(list);
        }

        public IActionResult Create()
        {
            var departments = _departamentService.FindAll();
            var viewModel = new SellerFormViewModel { Departaments = departments};
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Seller seller )
        {
            if (!ModelState.IsValid) { return View(new SellerFormViewModel { Seller = seller, Departaments = _departamentService.FindAll() }); }
            _sellerService.Insert(seller);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message="Id Not Provided."}); }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found." }); }
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            _sellerService.Remove(id);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Details(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided." }); }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found." }); }
            return View(obj);
        }

        public IActionResult Edit(int? id)
        {
            if (id == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Provided." }); }
            var obj = _sellerService.FindById(id.Value);
            if (obj == null) { return RedirectToAction(nameof(Error), new { message = "Id Not Found." }); }
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departaments = _departamentService.FindAll() };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Seller seller)
        {
            if (!ModelState.IsValid) { return View(new SellerFormViewModel { Seller = seller, Departaments = _departamentService.FindAll()}); }
            if (id != seller.Id) { return RedirectToAction(nameof(Error), new { message = "Id Mismatch." }); }
            try
            {
            _sellerService.Update(seller);
            return RedirectToAction(nameof(Index));
            }
            catch (ApplicationException e) { return RedirectToAction(nameof(Error), new { message = e.Message}); }
        }

        public IActionResult Error(string message)
        {
            var viewModel = new ErrorViewModel { Message = message, RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier};
            return View(viewModel);
        }

    }
}