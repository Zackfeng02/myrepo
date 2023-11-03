package com.zack.spring.mvc;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.context.annotation.Lazy;
import org.springframework.security.config.annotation.authentication.builders.AuthenticationManagerBuilder;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.servlet.util.matcher.MvcRequestMatcher;
import org.springframework.security.web.util.matcher.AntPathRequestMatcher;
import org.springframework.web.servlet.handler.HandlerMappingIntrospector;
import com.zack.spring.mvc.service.CustomerService;

@Configuration
@EnableWebSecurity
public class SecurityConfig {

    @Autowired
    private HandlerMappingIntrospector mvcHandlerMappingIntrospector;
    
    @Autowired
    private CustomerService customerService;
    
    @Autowired
    private CustomAuthenticationSuccessHandler customAuthenticationSuccessHandler;

    
    @Bean
    BCryptPasswordEncoder passwordEncoder() {
        return new BCryptPasswordEncoder();
    }
    
    @Bean
    SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        MvcRequestMatcher mvcRequestMatcher = new MvcRequestMatcher(mvcHandlerMappingIntrospector, "/register/**");
        
        http
            .authorizeHttpRequests((authorize) ->
                    authorize
            				.requestMatchers("/admin/").hasRole("ADMIN")
                    		.requestMatchers(mvcRequestMatcher).permitAll()
                    		.requestMatchers("/register", "/register/**").permitAll()
                    		.requestMatchers("/").permitAll()
                            .requestMatchers("/index").permitAll()
                            .requestMatchers("/css/**", "/js/**", "/images/**").permitAll()
                            .requestMatchers("/cus_overview", "/cus_overview/**").permitAll()
                            .requestMatchers("/acc_overview", "/acc_overview/**").permitAll()
            )
            .formLogin(form -> form
                            .loginPage("/login")
                            .loginProcessingUrl("/login")
                            .successHandler(customAuthenticationSuccessHandler)
                            .permitAll()
            )
            .logout(logout -> logout
                            .logoutRequestMatcher(new AntPathRequestMatcher("/logout"))
                            .permitAll()
            );
        return http.build();
    }

    @Autowired
    public void configureGlobal(AuthenticationManagerBuilder auth, @Lazy BCryptPasswordEncoder passwordEncoder) throws Exception {
        auth
            .userDetailsService(customerService)
            .passwordEncoder(passwordEncoder);
    }
}
