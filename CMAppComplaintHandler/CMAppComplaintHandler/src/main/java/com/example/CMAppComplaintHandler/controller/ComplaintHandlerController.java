import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import entity.ComplaintHandler;
import service.ComplaintHandlerService;

import java.util.List;

@RestController
@RequestMapping("/api/complaint-handlers")
public class ComplaintHandlerController {

    private final ComplaintHandlerService complaintHandlerService;

    @Autowired
    public ComplaintHandlerController(ComplaintHandlerService complaintHandlerService) {
        this.complaintHandlerService = complaintHandlerService;
    }

    @PostMapping
    public ResponseEntity<ComplaintHandler> createComplaintHandler(@RequestBody ComplaintHandler complaintHandler) {
        ComplaintHandler savedComplaintHandler = complaintHandlerService.saveComplaintHandler(complaintHandler);
        return ResponseEntity.ok(savedComplaintHandler);
    }

    @GetMapping
    public ResponseEntity<List<ComplaintHandler>> getAllComplaintHandlers() {
        return ResponseEntity.ok(complaintHandlerService.getAllComplaintHandlers());
    }

    @GetMapping("/{id}")
    public ResponseEntity<ComplaintHandler> getComplaintHandlerById(@PathVariable Long id) {
        return complaintHandlerService.getComplaintHandlerById(id)
                .map(ResponseEntity::ok)
                .orElse(ResponseEntity.notFound().build());
    }

    @PutMapping("/{id}")
    public ResponseEntity<ComplaintHandler> updateComplaintHandler(@PathVariable Long id, @RequestBody ComplaintHandler complaintHandler) {
        return ResponseEntity.ok(complaintHandlerService.updateComplaintHandler(id, complaintHandler));
    }

    @DeleteMapping("/{id}")
    public ResponseEntity<?> deleteComplaintHandler(@PathVariable Long id) {
        complaintHandlerService.deleteComplaintHandler(id);
        return ResponseEntity.ok().build();
    }

    // Additional endpoint methods can be added as needed
}
