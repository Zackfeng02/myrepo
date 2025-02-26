import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import entity.ComplaintHandler;
import repository.ComplaintHandlerRepository;

import java.util.List;
import java.util.Optional;

@Service
public class ComplaintHandlerService {

    private final ComplaintHandlerRepository complaintHandlerRepository;

    @Autowired
    public ComplaintHandlerService(ComplaintHandlerRepository complaintHandlerRepository) {
        this.complaintHandlerRepository = complaintHandlerRepository;
    }

    public ComplaintHandler saveComplaintHandler(ComplaintHandler complaintHandler) {
        return complaintHandlerRepository.save(complaintHandler);
    }

    public List<ComplaintHandler> getAllComplaintHandlers() {
        return complaintHandlerRepository.findAll();
    }

    public Optional<ComplaintHandler> getComplaintHandlerById(Long id) {
        return complaintHandlerRepository.findById(id);
    }

    public List<ComplaintHandler> getComplaintHandlerByName(String name) {
        return complaintHandlerRepository.findByName(name);
    }

    public void deleteComplaintHandler(Long id) {
        complaintHandlerRepository.deleteById(id);
    }

    public ComplaintHandler updateComplaintHandler(Long id, ComplaintHandler updatedComplaintHandler) {
        return complaintHandlerRepository.findById(id)
                .map(complaintHandler -> {
                    complaintHandler.setName(updatedComplaintHandler.getName());
                    // Additional fields to update
                    return complaintHandlerRepository.save(complaintHandler);
                })
                .orElseGet(() -> {
                    updatedComplaintHandler.setId(id);
                    return complaintHandlerRepository.save(updatedComplaintHandler);
                });
    }
}
