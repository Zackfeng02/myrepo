package com.zack.spring.mvc.controller;
import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.service.CustomerService;
import com.zack.spring.mvc.service.UserRegistrationService;

import jakarta.transaction.Transactional;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/customers")
public class CustomerController {
    @Autowired
    private CustomerService customerService;
    
    @Autowired
    private UserRegistrationService userRegistrationService;

    @GetMapping("/")
    public String listCustomers(Model model) {
        model.addAttribute("customers", customerService.getAllCustomers());
        return "customer-list";
    }

    @GetMapping("/view/{id}")
    public String viewCustomer(@PathVariable Long id, Model model) {
        model.addAttribute("customer", customerService.getCustomerById(id));
        return "customer-view";
    }

    @GetMapping("/customers/new")
    public String showForm(Model model) {
        Customer customer = new Customer();
        model.addAttribute("customer", customer);
        return "customer-form";
    }
    
    @Transactional
    @PostMapping("/customers/save")
    public String saveCustomer(@ModelAttribute Customer customer) {
        customerService.saveCustomer(customer);
        System.out.println("Customer: " + customer);  // Debug log
        return "redirect:/customers/";
    }

    @GetMapping("/delete/{id}")
    public String deleteCustomer(@PathVariable Long id) {
        customerService.deleteCustomer(id);
        return "redirect:/customers/";
    }
    
    @GetMapping("/register")
    public String register(Model model) {
        model.addAttribute("customer", new Customer());
        return "customer-form";
    }
    
    //below is for user registration
    @Transactional
    @PostMapping("/register")
    public String registerCustomer(@ModelAttribute Customer customer) {
        userRegistrationService.registerCustomer(customer);
        return "redirect:/login/";
    }
}
