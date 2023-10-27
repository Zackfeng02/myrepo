package com.zack.spring.mvc;

import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.config.annotation.web.configuration.WebSecurityConfigurerAdapter;

@Configuration
@EnableWebSecurity
public class SecurityConfig extends WebSecurityConfigurerAdapter {

    @Override
    protected void configure(HttpSecurity http) throws Exception {
        http
            .authorizeRequests()
                .antMatchers("/").permitAll()  // Allow access to the index page
                .anyRequest().authenticated()  // All other requests require authentication
            .and()
            .formLogin()
                .loginPage("/login")  // Custom login page, which is actually the index page in your case
                .permitAll();
    }
}

