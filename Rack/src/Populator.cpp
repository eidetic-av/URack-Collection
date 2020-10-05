#include "collection-ui.hpp"
#include "plugin.hpp"

struct Populator : URack::UModule {
  enum ParamIds {
    ROTATION_Z_PARAM,
    ROTATION_Y_PARAM,
    ROTATION_X_PARAM,
    SELECT_ATTEN_PARAM,
    SELECT_PARAM,
    ROTATION_SPREAD_PARAM,
    ROTATION_OCTAVES_PARAM,
    SHAPE_PARAM,
    SURFACE_PARAM,
    SHIFT_Y_ATTEN_PARAM,
    SHIFT_X_ATTEN_PARAM,
    SIZE_ATTEN_PARAM,
    SHIFT_Y_PARAM,
    SHIFT_X_PARAM,
    SIZE_SPREAD_PARAM,
    SIZE_PARAM,
    SHIFT_Z_ATTEN_PARAM,
    SHIFT_SPREAD_PARAM,
    STRETCH_Z_PARAM,
    STRETCH_Y_PARAM,
    STRETCH_X_PARAM,
    SHIFT_Z_PARAM,
    NUM_PARAMS
  };
  enum InputIds {
    POINT_CLOUD_INPUT,
    ROTATION_Z_INPUT,
    ROTATION_Y_INPUT,
    ROTATION_X_INPUT,
    SELECT_INPUT,
    ROTATION_SPREAD_INPUT,
    ROTATION_OCTAVES_INPUT,
    SHAPE_INPUT,
    SURFACE_INPUT,
    SHIFT_Y_INPUT,
    SHIFT_X_INPUT,
    SIZE_INPUT,
    SIZE_SPREAD_INPUT,
    SHIFT_SPREAD_INPUT,
    SHIFT_Z_INPUT,
    STRETCH_Z_INPUT,
    STRETCH_Y_INPUT,
    STRETCH_X_INPUT,
    NUM_INPUTS
  };
  enum OutputIds { NUM_OUTPUTS };
  enum LightIds { ACTIVE_LIGHT, NUM_LIGHTS };

  Populator() {
    config(NUM_PARAMS, NUM_INPUTS, NUM_OUTPUTS, NUM_LIGHTS);
    configBiUpdate("RotationZ", ROTATION_Z_PARAM, ROTATION_Z_INPUT);
    configBiUpdate("RotationY", ROTATION_Y_PARAM, ROTATION_Y_INPUT);
    configBiUpdate("RotationX", ROTATION_X_PARAM, ROTATION_X_INPUT);
    configUpdate("Select", SELECT_PARAM, SELECT_INPUT, SELECT_ATTEN_PARAM, 5.f);
    configUpdate("RotationSpread", ROTATION_SPREAD_PARAM,
                 ROTATION_SPREAD_INPUT);
    configUpdate("RotationOctaves", ROTATION_OCTAVES_PARAM,
                 ROTATION_OCTAVES_INPUT);
    configUpdate("Shape", SHAPE_PARAM, SHAPE_INPUT);
    configUpdate("Surface", SURFACE_PARAM, SURFACE_INPUT);
    configBiUpdate("ShiftY", SHIFT_Y_PARAM, SHIFT_Y_INPUT, SHIFT_Y_ATTEN_PARAM,
                   0.f);
    configBiUpdate("ShiftX", SHIFT_X_PARAM, SHIFT_X_INPUT, SHIFT_X_ATTEN_PARAM,
                   0.f);
    configUpdate("SizeSpread", SIZE_SPREAD_PARAM, SIZE_SPREAD_INPUT);
    configUpdate("Size", SIZE_PARAM, SIZE_INPUT, SIZE_ATTEN_PARAM, 0.f);
    configUpdate("ShiftSpread", SHIFT_SPREAD_PARAM, SHIFT_SPREAD_INPUT);
    configBiUpdate("StretchZ", STRETCH_Z_PARAM, STRETCH_Z_INPUT);
    configBiUpdate("StretchY", STRETCH_Y_PARAM, STRETCH_Y_INPUT);
    configBiUpdate("StretchX", STRETCH_X_PARAM, STRETCH_X_INPUT);
    configBiUpdate("ShiftZ", SHIFT_Z_PARAM, SHIFT_Z_INPUT, SHIFT_Z_ATTEN_PARAM,
                   0.f);
  }

  void update(const ProcessArgs &args) override {}
};

struct PopulatorWidget : URack::UModuleWidget {
  PopulatorWidget(Populator *module) {
    setModule(module);
    setPanel(APP->window->loadSvg(
        asset::plugin(pluginInstance, "res/Populator.svg")));

    addChild(createWidget<ScrewBlack>(Vec(RACK_GRID_WIDTH, 0)));
    addChild(
        createWidget<ScrewBlack>(Vec(box.size.x - 2 * RACK_GRID_WIDTH, 0)));
    addChild(createWidget<ScrewBlack>(
        Vec(RACK_GRID_WIDTH, RACK_GRID_HEIGHT - RACK_GRID_WIDTH)));
    addChild(createWidget<ScrewBlack>(Vec(box.size.x - 2 * RACK_GRID_WIDTH,
                                          RACK_GRID_HEIGHT - RACK_GRID_WIDTH)));

    addParam(createParam<PBlueKnobSmall>(mm2px(Vec(101.407, 19.92)), module,
                                         Populator::ROTATION_Z_PARAM));
    addParam(createParam<PBlueKnobSmall>(mm2px(Vec(82.818, 19.92)), module,
                                         Populator::ROTATION_Y_PARAM));
    addParam(createParam<PBlueKnobSmall>(mm2px(Vec(64.230, 19.920)), module,
                                         Populator::ROTATION_X_PARAM));
    addParam(createParam<PBlackKnobSmall>(mm2px(Vec(41.570, 19.92)), module,
                                          Populator::SELECT_ATTEN_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(33.676, 29.562)), module,
                                    Populator::SELECT_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(94.265, 43.981)), module,
                                    Populator::ROTATION_SPREAD_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(64.230, 43.981)), module,
                                    Populator::ROTATION_OCTAVES_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(14.087, 47.443)), module,
                                    Populator::SHAPE_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(33.676, 54.690)), module,
                                    Populator::SURFACE_PARAM));
    addParam(createParam<PBlackKnobSmall>(mm2px(Vec(102.158, 65.793)), module,
                                          Populator::SHIFT_Y_ATTEN_PARAM));
    addParam(createParam<PBlackKnobSmall>(mm2px(Vec(72.124, 65.793)), module,
                                          Populator::SHIFT_X_ATTEN_PARAM));
    addParam(createParam<PBlackKnobSmall>(mm2px(Vec(10.878, 68.869)), module,
                                          Populator::SIZE_ATTEN_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(94.265, 75.434)), module,
                                    Populator::SHIFT_Y_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(64.230, 75.434)), module,
                                    Populator::SHIFT_X_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(33.676, 78.511)), module,
                                    Populator::SIZE_SPREAD_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(14.087, 78.510)), module,
                                    Populator::SIZE_PARAM));
    addParam(createParam<PBlackKnobSmall>(mm2px(Vec(102.158, 97.036)), module,
                                          Populator::SHIFT_Z_ATTEN_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(64.230, 101.640)), module,
                                    Populator::SHIFT_SPREAD_PARAM));
    addParam(createParam<PBlueKnobSmall>(mm2px(Vec(49.147, 104.270)), module,
                                         Populator::STRETCH_Z_PARAM));
    addParam(createParam<PBlueKnobSmall>(mm2px(Vec(30.559, 104.270)), module,
                                         Populator::STRETCH_Y_PARAM));
    addParam(createParam<PBlueKnobSmall>(mm2px(Vec(11.97, 104.27)), module,
                                         Populator::STRETCH_X_PARAM));
    addParam(createParam<PBlueKnob>(mm2px(Vec(94.265, 106.677)), module,
                                    Populator::SHIFT_Z_PARAM));

    addPointCloudInput(mm2px(Vec(15.747, 28.189)), module,
                       Populator::POINT_CLOUD_INPUT, "PointCloudInput");
    addInput(createInput<PJ301MPort>(mm2px(Vec(110.075, 26.905)), module,
                                     Populator::ROTATION_Z_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(91.486, 26.905)), module,
                                     Populator::ROTATION_Y_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(72.898, 26.905)), module,
                                     Populator::ROTATION_X_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(49.146, 28.108)), module,
                                     Populator::SELECT_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(109.734, 43.981)), module,
                                     Populator::ROTATION_SPREAD_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(79.699, 43.981)), module,
                                     Populator::ROTATION_OCTAVES_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(3.514, 47.443)), module,
                                     Populator::SHAPE_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(49.146, 54.690)), module,
                                     Populator::SURFACE_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(109.734, 73.980)), module,
                                     Populator::SHIFT_Y_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(79.699, 73.980)), module,
                                     Populator::SHIFT_X_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(3.514, 77.056)), module,
                                     Populator::SIZE_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(49.146, 78.510)), module,
                                     Populator::SIZE_SPREAD_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(79.699, 101.640)), module,
                                     Populator::SHIFT_SPREAD_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(109.734, 105.223)), module,
                                     Populator::SHIFT_Z_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(40.690, 111.255)), module,
                                     Populator::STRETCH_Z_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(22.102, 111.255)), module,
                                     Populator::STRETCH_Y_INPUT));
    addInput(createInput<PJ301MPort>(mm2px(Vec(3.514, 111.255)), module,
                                     Populator::STRETCH_X_INPUT));
  }
};

Model *modelPopulator = createModel<Populator, PopulatorWidget>("Populator");
