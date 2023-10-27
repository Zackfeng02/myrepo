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
    @Autowired
    private AccountTypeService accountTypeService;

    // List all account types
    @GetMapping("/")
    public String listAccountTypes(Model model) {
        model.addAttribute("accountTypes", accountTypeService.getAllAccountTypes());
        return "account-type-list";
    }

    // View a single account type by ID
    @GetMapping("/view/{id}")
    public String viewAccountType(@PathVariable Long id, Model model) {
        model.addAttribute("accountType", accountTypeService.getAccountTypeById(id));
        return "account-type-view";
    }

    // Show form for creating a new account type
    @GetMapping("/new")
    public String showCreateForm(Model model) {
        model.addAttribute("accountType", new AccountType());
        return "account-type-form";
    }

    // Save a new account type
    @PostMapping("/save")
    public String saveAccountType(@ModelAttribute AccountType accountType) {
        accountTypeService.saveAccountType(accountType);
        return "redirect:/account-types/";
    }

    // Show form for updating an existing account type
    @GetMapping("/edit/{id}")
    public String showEditForm(@PathVariable Long id, Model model) {
        model.addAttribute("accountType", accountTypeService.getAccountTypeById(id));
        return "account-type-form";
    }

    // Update an existing account type
    @PostMapping("/update/{id}")
    public String updateAccountType(@PathVariable Long id, @ModelAttribute AccountType accountType) {
        accountType.setId(id);
        accountTypeService.saveAccountType(accountType);
        return "redirect:/account-types/";
    }

    // Delete an account type by ID
    @GetMapping("/delete/{id}")
    public String deleteAccountType(@PathVariable Long id) {
        accountTypeService.deleteAccountType(id);
        return "redirect:/account-types/";
    }
}
