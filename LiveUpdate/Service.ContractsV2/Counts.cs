namespace Service.Contracts
{
   using System.Runtime.Serialization;

   [DataContract]
   public class Counts
   {
      [DataMember]
      public int CurrentCount { get; set; }

      [DataMember]
      public int CurrentBig { get; set; }

      [DataMember]
      public int CurrentSmall { get; set; }
   }
}
