// step1 - import mongoose
import mongoose, {PassportLocalSchema} from "mongoose";
const Schema = mongoose.Schema;//alias for mongoose schema
import passportLocalMongoose from 'passport-local-mongoose';

//create a schema that matches the data
const UserSchema = new Schema
({
    DisplayName: String,
    username: String,
    EmailAddress: String,
    Created:
    {
       type:Date,
       default: Date.now()
    },
    Updated:
    {
      type:Date,
      default: Date.now()
    }


},
{
    collection:"users"

}
);

declare global
{
    export type UserDocument = mongoose.Document &
    {
        username: String,
        EmailAddress: String,
        DisplayName: String
    }
}



//step 3 - plugin the passport local mongoose module
UserSchema.plugin(passportLocalMongoose);
//step 4 - create a model using the Schema
const Model = mongoose.model("Users", UserSchema as PassportLocalSchema);
export default Model;