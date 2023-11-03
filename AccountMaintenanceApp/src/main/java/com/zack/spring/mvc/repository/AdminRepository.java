package com.zack.spring.mvc.repository;

import com.zack.spring.mvc.entity.Customer;
import jakarta.persistence.EntityManager;
import jakarta.persistence.Query;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Map;
import java.util.stream.Collectors;

@Repository
public class AdminRepository {

    @Autowired
    private EntityManager entityManager;

    public Map<String, Long> getCountByAccountType() {
        String sql = "SELECT a.account_type_code, COUNT(c.customer_id) " +
                     "FROM account a " +
                     "JOIN customer c ON a.customer_id = c.customer_id " +
                     "GROUP BY a.account_type_code";
        Query query = entityManager.createNativeQuery(sql);
        List<Object[]> results = query.getResultList();
        return results.stream().collect(Collectors.toMap(res -> (String) res[0], res -> (Long) res[1]));
    }

    public List<Customer> getCustomersWithHighBalance() {
        String sql = "SELECT c.* " +
                     "FROM customer c " +
                     "JOIN account a ON c.customer_id = a.customer_id " +
                     "WHERE a.account_type_code = 'SAV' AND a.balance > 1000";
        Query query = entityManager.createNativeQuery(sql, Customer.class);
        return query.getResultList();
    }

    public List<Customer> getCustomersWithOverdraft() {
        String sql = "SELECT c.* " +
                     "FROM customer c " +
                     "JOIN account a ON c.customer_id = a.customer_id " +
                     "WHERE a.over_draft_limit IS NOT NULL";
        Query query = entityManager.createNativeQuery(sql, Customer.class);
        return query.getResultList();
    }
}
