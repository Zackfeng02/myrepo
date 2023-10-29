package com.zack.spring.mvc.controller;

import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;

import com.zack.spring.mvc.entity.Customer;

@Controller
public class IndexController {

    @GetMapping("/")
    public String showIndexPage() {
        return "index";
    }
    
    @GetMapping("/login")
    public String showLoginPage() {
        return "index";
    }
    
    @GetMapping("/register")
    public String showRegistrationForm(Model model) {
        Customer customer = new Customer();
        model.addAttribute("customer", customer);
        return "registration-form";
    }
    
    @GetMapping("/cus_overview")
    public String customerOverview() {
        return "customer-overview";
    }
}
