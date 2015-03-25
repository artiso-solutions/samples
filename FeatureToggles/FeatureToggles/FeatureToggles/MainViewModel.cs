namespace SampleFeatureToggles
{
   using global::SampleFeatureToggles.FeatureToggles;

   public class MainViewModel
   {
      public LoadDataFeature LoadDataFeature
      {
         get
         {
            return new LoadDataFeature();
         }
      }
   }
}
