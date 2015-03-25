namespace SampleFeatureToggles
{
   using SampleFeatureToggles.FeatureToggles;

   public class MainViewModel
   {
      public LoadDataFeature LoadDataFeature
      {
         get
         {
            return new LoadDataFeature();
         }
      }

      public SaveDataFeature SaveDataFeature
      {
         get
         {
            return new SaveDataFeature();
         }
      }
   }
}
