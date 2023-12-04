package Entity;

import jakarta.persistence.*;

@Entity
@Table(name = "Customer")
public class Customer {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer customerId;

    @Column(length = 255)
    private String firstName;

    @Column(length = 255)
    private String lastName;

    @Column(length = 255)
    private String address;

    @Column(length = 20)
    private String phone;

    @Column(length = 255, unique = true)
    private String emailId;

    // Getters and setters

    public Integer getCustomerId() {
        return customerId;
    }

    public void setCustomerId(Integer id) {
        this.customerId = id;
    }

    public String getFirstName() {
        return firstName;
    }

    public void setFirstName(String name) {
        this.firstName = name;
    }

    public String getLastName() {
        return lastName;
    }

    public void setLastName(String name) {
        this.lastName = name;
    }

    public String getAddress() {
        return address;
    }

    public void setAddress(String add) {
        this.address = add;
    }

    public String getPhone() {
        return phone;
    }

    public void setPhone(String ph) {
        this.phone = ph;
    }

    public String getEmailId() {
        return emailId;
    }

    public void setEmailId(String email) {
        this.emailId = email;
    }
    
}

