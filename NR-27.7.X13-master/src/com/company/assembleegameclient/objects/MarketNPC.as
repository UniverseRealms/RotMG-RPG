package com.company.assembleegameclient.objects
{
import com.company.assembleegameclient.game.GameSprite;
import com.company.assembleegameclient.ui.panels.Panel;
import com.company.assembleegameclient.ui.tooltip.TextToolTip;
import com.company.assembleegameclient.ui.tooltip.ToolTip;
import kabam.rotmg.pets.view.components.PetInteractionPanel;
import kabam.rotmg.text.model.TextKey;

import com.company.assembleegameclient.game.GameSprite;
import Market.MarketNPCPanel;

public class MarketNPC extends GameObject implements IInteractiveObject
{


    public function MarketNPC(param1:XML)
    {
        super(param1);
        isInteractive_ = true;
    }

    public function getTooltip() : ToolTip
    {
        return new TextToolTip(3552822,10197915, TextKey.CLOSEDGIFTCHEST_TITLE, TextKey.TEXTPANEL_GIFTCHESTISEMPTY,200);
    }

    public function getPanel(param1:GameSprite) : Panel
    {
        return new MarketNPCPanel(param1,objectType_);
    }
}
}
