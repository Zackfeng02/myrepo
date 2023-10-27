package com.zack.spring.mvc.controller;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.service.CustomerService;

import java.util.List;
import java.util.Map;

@Controller
@RequestMapping("/admin")
public class AdminController {

    @Autowired
    private CustomerService customerService;

    @GetMapping("/")
    public String showAdminPage(Model model) {
        // Fetch the count of customers by account type
        Map<String, Long> countByAccountType = customerService.getCountByAccountType();
        
        // Fetch the list of customers whose savings account balance is more than $1000
        List<Customer> customersWithHighBalance = customerService.getCustomersWithHighBalance(1000);
        
        // Fetch the list of customers who have opted for an overdraft limit
        List<Customer> customersWithOverdraft = customerService.getCustomersWithOverdraft();

        // Add these to the model
        model.addAttribute("countByAccountType", countByAccountType);
        model.addAttribute("customersWithHighBalance", customersWithHighBalance);
        model.addAttribute("customersWithOverdraft", customersWithOverdraft);

        return "admin";
    }
}

