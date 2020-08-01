#include "plugin.hpp"
#include "collection-ui.hpp"

struct DayTripper : URack::UModule {
  enum ParamIds {
    PROGRAM_A_PARAM,
    PROGRAM_B_PARAM,
    PROGRAM_A_ATTEN_PARAM,
    PROGRAM_B_ATTEN_PARAM,
    ACTIVE_PARAM,
    NUM_PARAMS
  };
  enum InputIds { PROGRAM_A_INPUT, PROGRAM_B_INPUT, ACTIVE_INPUT, NUM_INPUTS };
  enum OutputIds { NUM_OUTPUTS };
  enum LightIds { ACTIVE_LIGHT, NUM_LIGHTS };

  DayTripper() {
    config(NUM_PARAMS, NUM_INPUTS, NUM_OUTPUTS, NUM_LIGHTS);
    configBiUpdate("ProgramA", PROGRAM_A_PARAM, PROGRAM_A_INPUT,
                   PROGRAM_A_ATTEN_PARAM, 0.f);
    configBiUpdate("ProgramB", PROGRAM_B_PARAM, PROGRAM_B_INPUT,
                   PROGRAM_B_ATTEN_PARAM, 0.f);
    configActivate(ACTIVE_PARAM, ACTIVE_LIGHT, ACTIVE_INPUT);
  }

  void update(const ProcessArgs &args) override {}
};

struct DayTripperWidget : URack::UModuleWidget {
  DayTripperWidget(DayTripper *module) {
    setModule(module);
    setPanel(APP->window->loadSvg(
        asset::plugin(pluginInstance, "res/DayTripper.svg")));

    addChild(createWidget<ScrewBlack>(Vec(RACK_GRID_WIDTH, 0)));
    addChild(
        createWidget<ScrewBlack>(Vec(box.size.x - 2 * RACK_GRID_WIDTH, 0)));
    addChild(createWidget<ScrewBlack>(
        Vec(RACK_GRID_WIDTH, RACK_GRID_HEIGHT - RACK_GRID_WIDTH)));
    addChild(createWidget<ScrewBlack>(Vec(box.size.x - 2 * RACK_GRID_WIDTH,
                                          RACK_GRID_HEIGHT - RACK_GRID_WIDTH)));

    addParam(createParamCentered<Davies1900hCustomWhiteKnob>(
        mm2px(Vec(16.515, 75.034)), module, DayTripper::PROGRAM_A_PARAM));
    addParam(createParamCentered<Davies1900hCustomWhiteKnob>(
        mm2px(Vec(44.444, 75.034)), module, DayTripper::PROGRAM_B_PARAM));
    addParam(createParamCentered<TrimpotGray>(
        mm2px(Vec(10.656, 95.994)), module, DayTripper::PROGRAM_A_ATTEN_PARAM));
    addParam(createParamCentered<TrimpotGray>(
        mm2px(Vec(38.586, 95.994)), module, DayTripper::PROGRAM_B_ATTEN_PARAM));
    addParam(createParamCentered<LEDBezel>(mm2px(Vec(24.451, 108.759)), module,
                                           DayTripper::ACTIVE_PARAM));
    addChild(createLightCentered<LEDBezelLight<RedLight>>(
        mm2px(Vec(24.451, 108.759)), module, DayTripper::ACTIVE_LIGHT));

    addInput(createInputCentered<PJ301MPort>(mm2px(Vec(21.345, 95.994)), module,
                                             DayTripper::PROGRAM_A_INPUT));
    addInput(createInputCentered<PJ301MPort>(mm2px(Vec(49.274, 95.994)), module,
                                             DayTripper::PROGRAM_B_INPUT));
    addInput(createInputCentered<PJ301MPort>(mm2px(Vec(36.003, 108.759)),
                                             module, DayTripper::ACTIVE_INPUT));
    auto sw = new ScreenSelector;
    addChild(sw);
    sw->box.pos = mm2px(Vec(14.158, 24.194));
    sw->addFrame(APP->window->loadSvg(
        asset::plugin(pluginInstance, "res/xr-platforms/hololens-2.svg")));
    sw->addFrame(APP->window->loadSvg(
        asset::plugin(pluginInstance, "res/ScreenSelectorPlaceholder.svg")));
  }

  struct ArrowButtonLeft : SvgButton {
    ArrowButtonLeft() {
      addFrame(APP->window->loadSvg(
          asset::plugin(pluginInstance, "res/ArrowButtonLeft.svg")));
    }
  };
  struct ArrowButtonRight : SvgButton {
    ArrowButtonRight() {
      addFrame(APP->window->loadSvg(
          asset::plugin(pluginInstance, "res/ArrowButtonRight.svg")));
    }
  };

  struct ScreenSelector : Widget {

    SvgWidget *image;
    std::vector<std::shared_ptr<Svg>> frames = {};
    unsigned int frameIndex = 0;

    struct LeftButton : ArrowButtonLeft {
      void onAction(const ::rack::event::Action &e) override {
        auto selector =
            dynamic_cast<ScreenSelector *>(e.getTarget()->parent->parent);
        // TODO: make this increment
        selector->decrementFrame();
      }
    };
    LeftButton *leftButton;

    struct RightButton : ArrowButtonRight {
      void onAction(const ::rack::event::Action &e) override {
        DEBUG("onRightAction");
      }
    };
    RightButton *rightButton;

    ScreenSelector() {
      image = new SvgWidget;
      leftButton = new LeftButton;
      rightButton = new RightButton;

      image->setSvg(APP->window->loadSvg(
          asset::plugin(pluginInstance, "res/ScreenSelectorPlaceholder.svg")));

      addChild(leftButton);
      leftButton->box.pos =
          Vec(0, image->box.getCenter().y - leftButton->box.getCenter().y);

      addChild(image);
      image->box.pos = Vec(22.5, 0);

      addChild(rightButton);
      rightButton->box.pos =
          Vec(image->box.size.x + 25,
              image->box.getCenter().y - rightButton->box.getCenter().y);
    }

    void decrementFrame() {
      image->setSvg(APP->window->loadSvg(
          asset::plugin(pluginInstance, "res/ScreenSelectorPlaceholder.svg")));
    }

    void addFrame(std::shared_ptr<Svg> frame) {
      if (frames.size() == 0)
        image->setSvg(frame);
      frames.push_back(frame);
    }
  };
};

Model *modelDayTripper =
    createModel<DayTripper, DayTripperWidget>("DayTripper");
