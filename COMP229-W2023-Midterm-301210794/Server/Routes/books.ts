// modules required for routing
import express from 'express';
const router = express.Router();
export default router;

// define the book model
import book from '../Models/books';

/* GET books List page. READ */

router.get('/', (req, res, next) => 
{
  // find all books in the books collection
  book.find( (err, books) => {
    if (err) {
      return console.error(err);
    }
    else {
      res.render('books/index', {
        title: 'Books',
        page: 'books',
        books: books
      });
    }
  });

});

//  GET the Book Details page in order to add a new Book
router.get('/add', (req, res, next) => {

    res.render('books/details',{title:'Add', page:'add', books:''});

});

// POST process the Book Details page and create a new Book - CREATE
router.post('/add', (req, res, next) => {

     // instantiate a new book object
   let newbook = new book
   ({
       Title: req.body.title,
       Price: req.body.price,
       Author: req.body.author,
       Genre: req.body.genre
   });
    book.create(newbook, function(err)
   {
       if(err)
       {
         
             console.error(err);
             res.end(err);     
       }
       return res.redirect('/books');

   });

});

// GET the Book Details page in order to edit an existing Book
router.get('/:id', (req, res, next) => {

    /*****************
     * ADD CODE HERE *
     *****************/
   let id = req.params.id;
   // pass the id to the db and read the contact into the edit page
   
   book.findById(id, {}, {}, function(err, bookToEdit)
   {
     if(err)
     {
       console.error(err);
       res.end(err);
     }
 
     // show the edit view with the data
     res.render('books/details', { title: 'Edit', page: 'edit', books:bookToEdit})
   });
});

// POST - process the information passed from the details form and update the document
router.post('/:id', (req, res, next) => {
   /*****************
     * ADD CODE HERE *
     *****************/
    let id = req.params.id;

   // instantiate a new book to Edit
   let updatedBook = new book
   ({
       _id: id,
       Title: req.body.title,
       Price: req.body.price,
       Author: req.body.author,
       Genre: req.body.genre
   });
 
   //update the book in database
   book.updateOne({_id: id}, updatedBook, function(err:ErrorCallback)
   {
     if(err)
     {
       console.error(err);
       res.end(err);
     }
 
     // edit was successful -> go to the books page
     res.redirect('/books');
   });

   

});

// GET - process the delete by user id
router.get('/delete/:id', (req, res, next) => {
   /*****************
     * ADD CODE HERE *
     *****************/
    let id = req.params.id;
    // pass the id to database and delete the book
  book.remove({_id:req.params.id}, function(err)
  {
    if(err)
    {
      console.error(err);
      res.end(err);
    }

    // delete was successful
    res.redirect('/books');});
});


module.exports = router;
