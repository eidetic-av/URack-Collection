#include "collection-ui.hpp"
#include "plugin.hpp"

struct Insider : URack::UModule {
  enum ParamIds {
    A_ATTEN_PARAM,
    A_PARAM,
    B_ATTEN_PARAM,
    B_PARAM,
    C_ATTEN_PARAM,
    C_PARAM,
    D_ATTEN_PARAM,
    D_PARAM,
    E_PARAM,
    E_ATTEN_PARAM,
    ACTIVE_PARAM,
    NUM_PARAMS
  };
  enum InputIds {
    A_INPUT,
    B_INPUT,
    C_INPUT,
    D_INPUT,
    E_INPUT,
    ACTIVE_INPUT,
    NUM_INPUTS
  };
  enum OutputIds { NUM_OUTPUTS };
  enum LightIds { ACTIVE_LIGHT, NUM_LIGHTS };

  std::string scriptTarget;
  std::string aTarget;
  std::string bTarget;
  std::string cTarget;
  std::string dTarget;
  std::string eTarget;
  bool sentInitialFieldUpdate;

  URack::UModuleWidget *widget;

  Insider() {
    config(NUM_PARAMS, NUM_INPUTS, NUM_OUTPUTS, NUM_LIGHTS);
    configBiUpdate("A", A_PARAM, A_INPUT, A_ATTEN_PARAM, 0.f);
    configBiUpdate("B", B_PARAM, B_INPUT, B_ATTEN_PARAM, 0.f);
    configBiUpdate("C", C_PARAM, C_INPUT, C_ATTEN_PARAM, 0.f);
    configBiUpdate("D", D_PARAM, D_INPUT, D_ATTEN_PARAM, 0.f);
    configBiUpdate("E", E_PARAM, E_INPUT, E_ATTEN_PARAM, 0.f);
    configActivate(ACTIVE_PARAM, ACTIVE_LIGHT, ACTIVE_INPUT);
  }

  void configCustomBiUpdate(std::string *inputField, ParamIds paramId,
                            InputIds inputId, ParamIds attenParamId) {}

  void update(const ProcessArgs &args) override {

    if (!sentInitialFieldUpdate) {
      widget->updateFields();
      sentInitialFieldUpdate = true;
    }
  }

  void onLoad(json_t *rootJ) override {
    json_t *targetJ = json_object_get(rootJ, "scriptTarget");
    if (targetJ)
      scriptTarget = json_string_value(targetJ);
    else
      scriptTarget = "";
    targetJ = json_object_get(rootJ, "aTarget");
    if (targetJ)
      aTarget = json_string_value(targetJ);
    else
      aTarget = "";
    targetJ = json_object_get(rootJ, "bTarget");
    if (targetJ)
      bTarget = json_string_value(targetJ);
    else
      bTarget = "";
    targetJ = json_object_get(rootJ, "cTarget");
    if (targetJ)
      cTarget = json_string_value(targetJ);
    else
      cTarget = "";
    targetJ = json_object_get(rootJ, "dTarget");
    if (targetJ)
      dTarget = json_string_value(targetJ);
    else
      dTarget = "";
    targetJ = json_object_get(rootJ, "eTarget");
    if (targetJ)
      eTarget = json_string_value(targetJ);
    else
      eTarget = "";
  }

  json_t *onSave() override {
    json_t *rootJ = json_object();
    json_object_set_new(rootJ, "scriptTarget",
                        json_string(scriptTarget.c_str()));
    json_object_set_new(rootJ, "aTarget", json_string(aTarget.c_str()));
    json_object_set_new(rootJ, "bTarget", json_string(bTarget.c_str()));
    json_object_set_new(rootJ, "cTarget", json_string(cTarget.c_str()));
    json_object_set_new(rootJ, "dTarget", json_string(dTarget.c_str()));
    json_object_set_new(rootJ, "eTarget", json_string(eTarget.c_str()));
    return rootJ;
  }
};

struct TargetField : TextField {
  Insider *module;
  std::string *string;
  std::string name;

  TargetField(Insider *_module, std::string *_string, std::string _name,
              std::string _placeholder) {
    box.size.x = 100;
    module = _module;
    placeholder = _placeholder;
    name = _name;
    string = _string;
    if (!string->empty())
      update(*string);
  }

  void update(std::string newValue) {
    setText(newValue);
    string->assign(newValue);
    URack::networkManager->dispatcher->action(
        module->activeHosts, module->instanceAddress + "/UpdateTarget",
        name + "/" + newValue);
  }

  void onSelectKey(const event::SelectKey &e) override {
    if (e.action == GLFW_RELEASE)
      update(text);
    else if (e.action == GLFW_PRESS && e.key == GLFW_KEY_BACKSPACE)
      setText(text.substr(0, text.size() - 1));
    e.consume(this);
  }
};

struct InsiderWidget : URack::UModuleWidget {
  TargetField *targetField;
  TargetField *aField;
  TargetField *bField;
  TargetField *cField;
  TargetField *dField;
  TargetField *eField;

  InsiderWidget(Insider *module) {
    setModule(module);
    setPanel(
        APP->window->loadSvg(asset::plugin(pluginInstance, "res/Insider.svg")));

    addParam(createParamCentered<LEDBezel>(mm2px(Vec(40, 13)), module,
                                           Insider::ACTIVE_PARAM));
    addChild(createLightCentered<LEDBezelLight<RedLight>>(
        mm2px(Vec(40, 13)), module, Insider::ACTIVE_LIGHT));

    addInput(createInputCentered<PJ301MPort>(mm2px(Vec(8, 28)), module,
                                             Insider::A_INPUT));
    addParam(createParamCentered<TrimpotGray>(mm2px(Vec(18, 28)), module,
                                              Insider::A_ATTEN_PARAM));
    addParam(createParamCentered<Davies1900hSmallWhiteKnob>(
        mm2px(Vec(28, 28)), module, Insider::A_PARAM));

    addInput(createInputCentered<PJ301MPort>(mm2px(Vec(8, 48)), module,
                                             Insider::B_INPUT));
    addParam(createParamCentered<TrimpotGray>(mm2px(Vec(18, 48)), module,
                                              Insider::B_ATTEN_PARAM));
    addParam(createParamCentered<Davies1900hSmallWhiteKnob>(
        mm2px(Vec(28, 48)), module, Insider::B_PARAM));

    addInput(createInputCentered<PJ301MPort>(mm2px(Vec(8, 68)), module,
                                             Insider::C_INPUT));
    addParam(createParamCentered<TrimpotGray>(mm2px(Vec(18, 68)), module,
                                              Insider::C_ATTEN_PARAM));
    addParam(createParamCentered<Davies1900hSmallWhiteKnob>(
        mm2px(Vec(28, 68)), module, Insider::C_PARAM));

    addInput(createInputCentered<PJ301MPort>(mm2px(Vec(8, 88)), module,
                                             Insider::D_INPUT));
    addParam(createParamCentered<TrimpotGray>(mm2px(Vec(18, 88)), module,
                                              Insider::D_ATTEN_PARAM));
    addParam(createParamCentered<Davies1900hSmallWhiteKnob>(
        mm2px(Vec(28, 88)), module, Insider::D_PARAM));

    addInput(createInputCentered<PJ301MPort>(mm2px(Vec(8, 108)), module,
                                             Insider::E_INPUT));
    addParam(createParamCentered<TrimpotGray>(mm2px(Vec(18, 108)), module,
                                              Insider::E_ATTEN_PARAM));
    addParam(createParamCentered<Davies1900hSmallWhiteKnob>(
        mm2px(Vec(28, 108)), module, Insider::E_PARAM));

    if (module) {
      auto m = (Insider *)module;
      m->widget = this;
      targetField =
          new TargetField(m, &m->scriptTarget, "TargetName", "Target");
      targetField->setPosition(mm2px(Vec(2, 10)));
      addChild(targetField);

      aField = new TargetField(m, &m->aTarget, "A", "Property");
      aField->setPosition(mm2px(Vec(4, 35)));
      addChild(aField);

      bField = new TargetField(m, &m->bTarget, "B", "Property");
      bField->setPosition(mm2px(Vec(4, 55)));
      addChild(bField);

      cField = new TargetField(m, &m->cTarget, "C", "Property");
      cField->setPosition(mm2px(Vec(4, 75)));
      addChild(cField);

      dField = new TargetField(m, &m->dTarget, "D", "Property");
      dField->setPosition(mm2px(Vec(4, 95)));
      addChild(dField);

      eField = new TargetField(m, &m->eTarget, "E", "Property");
      eField->setPosition(mm2px(Vec(4, 115)));
      addChild(eField);
    }
  }

  void updateFields() override {
    auto m = (Insider *)this->module;
    targetField->update(m->scriptTarget);
    aField->update(m->aTarget);
    bField->update(m->bTarget);
    cField->update(m->cTarget);
    dField->update(m->dTarget);
    eField->update(m->eTarget);
  }
};

Model *modelInsider = createModel<Insider, InsiderWidget>("Insider");
