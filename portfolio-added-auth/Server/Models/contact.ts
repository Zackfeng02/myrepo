import mongoose from "mongoose";
const Schema = mongoose.Schema;
const ContactSchema = new Schema
({ 
    Name: String,
    Phone: String,
    Email : String
},
{
    collection:"contacts"

}
);

const Model = mongoose.model("Contact", ContactSchema);
export default Model;