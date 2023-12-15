import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;
import entity.ComplaintHandler;

import java.util.List;
import java.util.Optional;

@Repository
public interface ComplaintHandlerRepository extends JpaRepository<ComplaintHandler, Long> {

    // Find a ComplaintHandler by their name
    List<ComplaintHandler> findByName(String name);

    // Example of a custom query method (if needed)
    // Optional<ComplaintHandler> findByEmail(String email);
}
