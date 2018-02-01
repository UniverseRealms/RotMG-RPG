package Market
{
   import robotlegs.bender.bundles.mvcs.Mediator
   import kabam.rotmg.dialogs.control.CloseDialogsSignal
   import flash.display.BitmapData;
   import com.company.assembleegameclient.objects.ObjectLibrary;
   
   public class InfoDialogEvents extends Mediator
   {
       
      
      [Inject]
      public var view:Market.InfoDialog;
      
      [Inject]

      [Inject]
      public var closeDialogs:CloseDialogsSignal;
      
      public function InfoDialogEvents()
      {
         super();
      }
      
      override public function initialize() : void
      {
         this.view.closed.addOnce(this._qN_);
         this.view._0K_L_(this._05V_());
      }
      
      private function _05V_() : BitmapData
      {
         var _loc1_:int = 403;
         return ObjectLibrary.getRedrawnTextureFromType(_loc1_,80,true);
      }
      
      override public function destroy() : void
      {
         this.view.closed.removeAll();
      }
      
      private function _qN_() : void
      {
         this.closeDialogs.dispatch();
      }
   }
}
