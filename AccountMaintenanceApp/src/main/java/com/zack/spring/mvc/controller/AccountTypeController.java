package com.zack.spring.mvc.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

import com.zack.spring.mvc.entity.AccountType;
import com.zack.spring.mvc.service.AccountTypeService;

@Controller
@RequestMapping("/account-types")
public class AccountTypeController {
    //@Autowired
    //private AccountTypeService accountTypeService;

    // List all account types
    //@GetMapping("/")
    //public String listAccountTypes(Model model) {
       //model.addAttribute("accountTypeName", accountTypeService.getAllAccountTypes());
        //return "account-type-list";
    //}

    // Show form for creating a new account type
    //@GetMapping("/new")
    //public String showCreateForm(Model model) {
        //model.addAttribute("accountTypeName", new AccountType());
        //return "account-type-form";
    //}

    // Save a new account type
    //@PostMapping("/save")
    //public String saveAccountType(@ModelAttribute AccountType accountType) {
        //accountTypeService.saveAccountType(accountType);
        //return "redirect:/account-types/";
    //}

    // Show form for updating an existing account type
    //@GetMapping("/edit/{id}")
    //public String showEditForm(@PathVariable String accountTypeCode, Model model) {
        //model.addAttribute("accountType", accountTypeService.getAccountTypeByCode(accountTypeCode));
        //return "account-type-form";
    //}

    // Update an existing account type
    //@PostMapping("/update/{id}")
    //public String updateAccountType(@PathVariable String accountTypeCode, @ModelAttribute AccountType accountType) {
        //accountType.setAccountTypeCode(accountTypeCode);
        //accountTypeService.saveAccountType(accountType);
       // return "redirect:/account-types/";
    //}

    // Delete an account type by ID
    //@GetMapping("/delete/{id}")
    //public String deleteAccountType(@PathVariable String accountTypeCode) {
       // accountTypeService.deleteAccountType(accountTypeCode);
        //return "redirect:/account-types/";
    //}
}
