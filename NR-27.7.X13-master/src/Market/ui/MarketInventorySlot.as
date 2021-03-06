package Market.ui
{
import com.company.assembleegameclient.util.DisplayHierarchy;

import flash.events.Event;
import com.company.assembleegameclient.objects.ObjectLibrary;
import flash.events.MouseEvent;
import Market.ObjectSlot;

import kabam.rotmg.constants.ItemConstants;

import kabam.rotmg.pets.view.FeedPetView;
import kabam.rotmg.pets.view.FusePetView;

import kabam.rotmg.pets.view.components.slot.FeedFuseSlot;
import kabam.rotmg.questrewards.components.ModalItemSlot;

public class MarketInventorySlot extends FeedFuseSlot
   {

       private var unblockItemUpdates:Function;

       public function MarketInventorySlot()
       {
         super();
         itemSprite.addEventListener(MouseEvent.MOUSE_DOWN,this.onMouseDown);
         this.updateTitle();
       }

       override protected function onRemovedFromStage(_arg1:Event):void {
           super.onRemovedFromStage(_arg1);
           this.unblockSlot();
       }
      
      public function setItem(param1:int, param2:int, param3:int) : void
      {
         if(this.itemId != param1)
         {
            this.unblockSlot();
            this.itemId = param1;
            this.slotId = param2;
            this.objectId = param3;
            itemBitmap.bitmapData = ObjectLibrary.getRedrawnTextureFromType(param1,80,true);
            alignBitmapInBox();
            this.updateTitle();
         }
      }
      
      public function updateTitle() : void
      {
         var _loc1_:XML = null;
         var _loc2_:String = null;
         if(itemId && itemId != -1)
         {
            setTitle("Ready to sell",{});
            _loc1_ = ObjectLibrary.parseFromXML[itemId];
            _loc2_ = !!_loc1_.hasOwnProperty("DisplayId")?_loc1_.DisplayId:_loc1_.@id;
            setSubtitle(_loc2_,{});
         }
         else
         {
            setTitle("",{});
            setSubtitle("Drag an item here if you wish to sell it",{});
         }
      }
      
      public function clearSlot() : void
      {
         this.unblockSlot();
         itemId = -1;
         itemBitmap.bitmapData = null;
         slotId = -1;
         objectId = -1;
         this.updateTitle();
      }
      
      private function alignImage(param1:int, param2:int) : void
      {
         itemBitmap.x = -itemBitmap.width / 2;
         itemBitmap.y = -itemBitmap.height / 2;
         itemSprite.x = param1;
         itemSprite.y = param2;
      }
      
      private function onMouseDown(param1:MouseEvent) : void
      {
         this.alignImage(param1.stageX,param1.stageY);
         itemSprite.startDrag(true);
         itemSprite.addEventListener(MouseEvent.MOUSE_UP,this.onMouseUp);
         if(itemSprite.parent != null && itemSprite.parent != stage)
         {
            removeChild(itemSprite);
            stage.addChild(itemSprite);
         }
      }
      
      private function onMouseUp(param1:MouseEvent) : void {
          itemSprite.stopDrag();
          itemSprite.removeEventListener(MouseEvent.MOUSE_UP, onMouseUp);
          stage.removeChild(this.itemSprite);
          alignBitmapInBox();
          var _loc2_:* = (itemSprite.dropTarget, MarketInventorySlot);
          if (!(_loc2_ is MarketInventorySlot)) {
              clearSlot();
          }
      }

      private function unblockSlot() : void
      {
         this.unblockItemUpdates && this.unblockItemUpdates();
         this.unblockItemUpdates = null;
      }

      public function makeObjectSlot() : ObjectSlot
      {
         var _loc1_:ObjectSlot = new ObjectSlot();
         _loc1_.objectId_ = objectId;
         _loc1_.objectType_ = itemId;
         _loc1_.slotId = slotId;
         return _loc1_;
      }
   }
}
