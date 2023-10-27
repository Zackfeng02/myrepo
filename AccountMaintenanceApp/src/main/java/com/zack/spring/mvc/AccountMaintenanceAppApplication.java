package com.zack.spring.mvc;

import org.springframework.boot.SpringApplication;
import org.springframework.boot.autoconfigure.SpringBootApplication;
import org.springframework.boot.autoconfigure.domain.EntityScan;
import org.springframework.data.jpa.repository.config.EnableJpaRepositories;

@SpringBootApplication
@EnableJpaRepositories("com.zack.spring.mvc.repository")
@EntityScan(basePackages = "com.zack.spring.mvc.entity")
public class AccountMaintenanceAppApplication {

	public static void main(String[] args) {
		SpringApplication.run(AccountMaintenanceAppApplication.class, args);

	}

	
}
