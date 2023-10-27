package com.zack.spring.mvc.controller;
import com.zack.spring.mvc.entity.Account;
import com.zack.spring.mvc.service.AccountService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;


@Controller
@RequestMapping("/accounts")
public class AccountController {
    @Autowired
    private AccountService accountService;

    // List all accounts
    @GetMapping("/")
    public String listAccounts(Model model) {
        model.addAttribute("accounts", accountService.getAllAccounts());
        return "account-list";
    }

    // View a single account by ID
    @GetMapping("/view/{id}")
    public String viewAccount(@PathVariable Long id, Model model) {
        model.addAttribute("account", accountService.getAccountById(id));
        return "account-view";
    }

    // Show form for creating a new account
    @GetMapping("/new")
    public String showCreateForm(Model model) {
        model.addAttribute("account", new Account());
        return "account-form";
    }

    // Save a new account
    @PostMapping("/save")
    public String saveAccount(@ModelAttribute Account account) {
        accountService.saveAccount(account);
        return "redirect:/accounts/";
    }

    // Show form for updating an existing account
    @GetMapping("/edit/{id}")
    public String showEditForm(@PathVariable Long id, Model model) {
        model.addAttribute("account", accountService.getAccountById(id));
        return "account-form";
    }

    // Update an existing account
    @PostMapping("/update/{id}")
    public String updateAccount(@PathVariable Long id, @ModelAttribute Account account) {
        account.setId(id);
        accountService.saveAccount(account);
        return "redirect:/accounts/";
    }

    // Delete an account by ID
    @GetMapping("/delete/{id}")
    public String deleteAccount(@PathVariable Long id) {
        accountService.deleteAccount(id);
        return "redirect:/accounts/";
    }
}
