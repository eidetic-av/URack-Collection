#pragma once

#include "plugin.hpp"

struct Davies1900hCustomWhiteKnob : app::SvgKnob {
    Davies1900hCustomWhiteKnob() {
        minAngle = -0.75 * M_PI;
        maxAngle = 0.75 * M_PI;
        setSvg(APP->window->loadSvg(asset::plugin(pluginInstance, "res/Davies1900hWhite.svg")));
    }
};

struct Davies1900hSmallWhiteKnob : app::SvgKnob {
    Davies1900hSmallWhiteKnob() {
        minAngle = -0.75 * M_PI;
        maxAngle = 0.75 * M_PI;
        setSvg(APP->window->loadSvg(asset::plugin(pluginInstance, "res/Davies1900hSmallWhite.svg")));
    }
};

struct TrimpotGray : app::SvgKnob {
    TrimpotGray() {
        minAngle = -0.75 * M_PI;
        maxAngle = 0.75 * M_PI;
        setSvg(APP->window->loadSvg(asset::plugin(pluginInstance, "res/TrimpotGray.svg")));
    }
};

struct WhiteHorizontalSlider : app::SvgSlider {
    WhiteHorizontalSlider() {
        horizontal = true;
        minHandlePos = app::mm2px(math::Vec(-2.25, -3.5625));
        maxHandlePos = app::mm2px(math::Vec(24.75, -3.5625));
        setBackgroundSvg(APP->window->loadSvg(asset::plugin(pluginInstance, "res/WhiteHorizontalSlider.svg")));
        setHandleSvg(APP->window->loadSvg(asset::plugin(pluginInstance, "res/WhiteHorizontalSliderHandle.svg")));
        box.size = background->box.size.plus(app::mm2px(math::Vec(0, 4.75)));
    }
};
