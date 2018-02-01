package Market.ui
{
/*   import _4Z_._08i;

import com.company.assembleegameclient.account.ui.Frame;
import com.company.assembleegameclient.ui.panels.itemgrids.itemtiles.ItemTile;

import flash.events.Event;
   import com.company.assembleegameclient.objects.ObjectLibrary;
   import flash.events.MouseEvent;
   import com.company.assembleegameclient.util._P_K_;
   import Market.ObjectSlot;

import starling.display.Sprite;

public class MarketInventorySlot extends ItemTile
   {

       protected var itemSprite:Sprite;

       private var unblockItemUpdates:Function;
      
      public function MarketInventorySlot()
      {
         super();
         itemSprite.addEventListener(MouseEvent.MOUSE_DOWN,this.onMouseDown);
         this.updateTitle();
      }
      
      override protected function onRemovedFromStage(param1:Event) : void
      {
         super.onRemovedFromStage(param1);
         this.unblockSlot();
      }
      
      public function setItem(param1:int, param2:int, param3:int, param4:Function) : void
      {
         if(this.itemId != param1)
         {
            this.unblockSlot();
            this.itemId = param1;
            this.slotId = param2;
            this.objectId = param3;
            itemSlotSprite.bitmapData = ObjectLibrary.getRedrawnTextureFromType(param1,80,true);
            _0du();
            this.updateTitle();
            this.unblockItemUpdates = param4;
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
            setDescription(_loc2_,{});
         }
         else
         {
            setTitle("",{});
            setDescription("Drag an item here if you wish to sell it",{});
         }
      }
      
      public function clearSlot() : void
      {
         this.unblockSlot();
         itemId = -1;
         itemSlotSprite.bitmapData = null;
         slotId = -1;
         objectId = -1;
         this.updateTitle();
      }
      
      private function alignImage(param1:int, param2:int) : void
      {
         itemSlotSprite.x = -itemSlotSprite.width / 2;
         itemSlotSprite.y = -itemSlotSprite.height / 2;
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
      
      private function onMouseUp(param1:MouseEvent) : void
      {
         itemSprite.stopDrag();
         itemSprite.removeEventListener(MouseEvent.MOUSE_UP,this.onMouseUp);
         stage.removeChild(itemSprite);
         addChild(itemSprite);
         _0du();
         var _loc2_:* = _P_K_._0Z_w(itemSprite.dropTarget,MarketInventorySlot);
         if(!(_loc2_ is MarketInventorySlot))
         {
            this.unblockSlot();
            itemId = -1;
            itemSlotSprite.bitmapData = null;
            this.updateTitle();
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
   } */
}
