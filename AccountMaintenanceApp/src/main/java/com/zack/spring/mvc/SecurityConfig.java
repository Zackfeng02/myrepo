package com.zack.spring.mvc;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;
import org.springframework.security.config.annotation.authentication.builders.AuthenticationManagerBuilder;
import org.springframework.security.config.annotation.web.builders.HttpSecurity;
import org.springframework.security.config.annotation.web.configuration.EnableWebSecurity;
import org.springframework.security.core.userdetails.UserDetailsService;
import org.springframework.security.crypto.bcrypt.BCryptPasswordEncoder;
import org.springframework.security.crypto.password.PasswordEncoder;
import org.springframework.security.web.SecurityFilterChain;
import org.springframework.security.web.servlet.util.matcher.MvcRequestMatcher;
import org.springframework.security.web.util.matcher.AntPathRequestMatcher;
import org.springframework.web.servlet.handler.HandlerMappingIntrospector;

@Configuration
@EnableWebSecurity
public class SecurityConfig {

    @Autowired
    private UserDetailsService userDetailsService;

    @Autowired
    private HandlerMappingIntrospector mvcHandlerMappingIntrospector;

    @Bean
    static PasswordEncoder passwordEncoder(){
        return new BCryptPasswordEncoder();
    }

    @Bean
    SecurityFilterChain filterChain(HttpSecurity http) throws Exception {
        MvcRequestMatcher mvcRequestMatcher = new MvcRequestMatcher(mvcHandlerMappingIntrospector, "/register/**");
        
        http
            .authorizeHttpRequests((authorize) ->
                    authorize.requestMatchers(mvcRequestMatcher).permitAll()
                            .requestMatchers("/index").permitAll()
                            .requestMatchers("/users").hasRole("ADMIN")
            ).formLogin(
                    form -> form
                            .loginPage("/login")
                            .loginProcessingUrl("/login")
                            .defaultSuccessUrl("/users")
                            .permitAll()
            ).logout(
                    logout -> logout
                            .logoutRequestMatcher(new AntPathRequestMatcher("/logout"))
                            .permitAll()
            );
        return http.build();
    }

    @Autowired
    public void configureGlobal(AuthenticationManagerBuilder auth) throws Exception {
        auth
            .userDetailsService(userDetailsService)
            .passwordEncoder(passwordEncoder());
    }
}

