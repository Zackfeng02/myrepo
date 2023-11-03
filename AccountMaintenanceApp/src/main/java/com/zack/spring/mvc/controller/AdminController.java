package com.zack.spring.mvc.controller;

import com.zack.spring.mvc.service.AdminService;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Controller;
import org.springframework.ui.Model;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;

@Controller
@RequestMapping("/admin")
public class AdminController {

    @Autowired
    private AdminService adminService;

    @GetMapping("/")
    public String showAdminPage(Model model) {
        model.addAttribute("countByAccountType", adminService.getCountByAccountType());
        model.addAttribute("customersWithHighBalance", adminService.getCustomersWithHighBalance());
        model.addAttribute("customersWithOverdraft", adminService.getCustomersWithOverdraft());
        return "admin";
    }
}
