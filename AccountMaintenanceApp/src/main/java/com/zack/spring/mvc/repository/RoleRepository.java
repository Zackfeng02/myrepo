package com.zack.spring.mvc.repository;

import com.zack.spring.mvc.entity.Role;
import org.springframework.data.jpa.repository.JpaRepository;

public interface RoleRepository extends JpaRepository<Role, Long> {
    
    // Custom query to find a role by its name
    Role findByName(String name);
    
}
