export class Patient{
  constructor(
    public id : number,
    public firstName : string, 
    public lastName : string,
    public genderDescription : string,
    public birthDate : Date,
    public dateCreated : Date,
    public dateUpdated : Date){}
}