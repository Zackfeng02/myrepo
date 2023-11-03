package com.zack.spring.mvc.controller;
import com.zack.spring.mvc.entity.Account;
import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.service.AccountService;
import com.zack.spring.mvc.service.CustomerService;
import jakarta.transaction.Transactional;

import java.security.Principal;
import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.*;

@Controller
@RequestMapping("/cus_overview")
public class CustomerController {
    @Autowired
    private CustomerService customerService;
    
    @Autowired
    private AccountService accountService;
    
    @Autowired
    private BCryptPasswordEncoder bCryptPasswordEncoder;

    //@GetMapping("/view/{id}")
    //public String viewCustomer(@PathVariable Integer customer_id, Model model) {
        //model.addAttribute("customer", customerService.getCustomerById(customer_id));
        //return "customer-view";
    //}
    
    @GetMapping("/")
    public String customerOverview(Model model, Principal principal) {
        String username = principal.getName();
        
        Customer customer = customerService.findByUsername(username);

        model.addAttribute("customer", customer);
        
        List<Account> accounts = accountService.getAccountById(customer.getCustomerId());

            // Check if the accounts list is null or empty
            if (accounts != null && !accounts.isEmpty()) {
                model.addAttribute("accounts", accounts);
            } else {
                // Handle the case where accounts are not found
                model.addAttribute("errorMessage", "No accounts found for this customer.");
                }
        
        return "customer-overview";
    }



    // New customer creation
    @GetMapping("/register")
    public String showForm(Model model) {
        Customer customer = new Customer();
        model.addAttribute("customer", customer);
        return "customer-form";
    }

    @Transactional
    @PostMapping("/register")
    public String regCustomer(@ModelAttribute Customer customer) {
        customerService.saveCustomer(customer);
        return "redirect:/login";
    }
    
    // Customer information view only
    @GetMapping("/view/{customer_id}")
    public String showViewForm(@PathVariable Long customer_id, Model model) {
        model.addAttribute("customer", customerService.getCustomerById(customer_id));
        return "customer-details";
    }

    // Customer information edit
    @GetMapping("/edit/{customer_id}")
    public String showEditForm(@PathVariable Long customer_id, Model model) {
        model.addAttribute("customer", customerService.getCustomerById(customer_id));
        return "customer-form";
    }

    @Transactional
    @PostMapping("/save")
    public String updateCustomer(@ModelAttribute Customer customer) {
        
    	// Encode the password using BCrypt
        String encodedPassword = bCryptPasswordEncoder.encode(customer.getPassword());
        // Set the encoded password back to the customer object
        customer.setPassword(encodedPassword);
    	customerService.saveCustomer(customer);
        return "redirect:/cus_overview";
    }

    // Delete customer
    @Transactional
    @GetMapping("/delete/{customer_id}")
    public String deleteCustomer(@PathVariable("customer_id") Long customer_id) {
        customerService.deleteCustomer(customer_id);
        return "redirect:/login";
    }
}
