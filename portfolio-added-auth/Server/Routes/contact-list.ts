import express from 'express';
import { AuthGuard } from '../Util';

const router = express.Router();
import {DisplayContactList, DisplayAddPage, ProcessAddPage, DisplayEditPage, ProcessEditPage, ProcessDeletePage} from '../Controllers/contact-list';
//display contact-list page
router.get('/contact-list', AuthGuard, DisplayContactList);
/* Display Add Page */
router.get('/add', AuthGuard, DisplayAddPage);
/* Display Edit Page */
router.get('/edit/:id', AuthGuard, DisplayEditPage);
/* Process Add Page */
router.post('/add', AuthGuard, ProcessAddPage);
/* Process Edit Page */
router.post('/edit/:id', AuthGuard, ProcessEditPage);
/* Process Delete Page */
router.get('/delete/:id', AuthGuard, ProcessDeletePage);


export default router;