package com.zack.spring.mvc;

import java.util.Set;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.security.core.Authentication;
import org.springframework.security.web.authentication.AuthenticationSuccessHandler;
import org.springframework.stereotype.Component;
import com.zack.spring.mvc.entity.Customer;
import com.zack.spring.mvc.entity.Role;
import com.zack.spring.mvc.repository.CustomerRepository;
import jakarta.servlet.ServletException;
import jakarta.servlet.http.HttpServletRequest;
import jakarta.servlet.http.HttpServletResponse;
import jakarta.transaction.Transactional;

import java.io.IOException;

@Component
public class CustomAuthenticationSuccessHandler implements AuthenticationSuccessHandler {

    @Autowired
    private CustomerRepository customerRepository;

    @Override
    @Transactional
    public void onAuthenticationSuccess(HttpServletRequest request, HttpServletResponse response,
                                        Authentication authentication) throws IOException, ServletException {
        String username = authentication.getName();
        Customer customer = customerRepository.findByUsername(username);

        if (customer != null) {
            Set<Role> roles = customer.getRoles();

            // Check if the customer has the ADMIN role
            if (roles.stream().anyMatch(role -> "ADMIN".equals(role.getName()))) {
                response.sendRedirect("/admin/");
                return;
            }
        }
        response.sendRedirect("/cus_overview");
    }


}
