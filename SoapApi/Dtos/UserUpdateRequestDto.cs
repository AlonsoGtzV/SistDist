using System.Runtime.Serialization;
using System.ServiceModel;
using SoapApi.Models;

namespace SoapApi.Dtos;

[DataContract]
public class UserUpdateRequestDto{
    [DataMember]
    public Guid Id {get; set;}
    [DataMember]
    public string FirstName {get; set;} = null;
    [DataMember]
    public string LastName {get; set;} = null;
    [DataMember]
    public DateTime BirthDate {get; set;}
    
}