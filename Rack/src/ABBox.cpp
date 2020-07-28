#include "UModule.hpp"

struct ABBox : URack::UModule {
	enum ParamIds {
		POS_X_ATTEN_PARAM,
		POS_X_PARAM,
		POS_Y_ATTEN_PARAM,
		POS_Y_PARAM,
		POS_Z_ATTEN_PARAM,
		POS_Z_PARAM,
		SCALE_X_ATTEN_PARAM,
		SCALE_X_PARAM,
		SCALE_Y_ATTEN_PARAM,
		SCALE_Y_PARAM,
		SCALE_Z_ATTEN_PARAM,
		SCALE_Z_PARAM,
		NUM_PARAMS
	};
	enum InputIds {
		POS_X_INPUT,
		POS_Y_INPUT,
		POS_Z_INPUT,
		SCALE_X_INPUT,
		SCALE_Y_INPUT,
		SCALE_Z_INPUT,
		POINT_CLOUD_INPUT,
		NUM_INPUTS
	};
	enum OutputIds { POINTS_OUTSIDE_OUTPUT, POINTS_INSIDE_OUTPUT, NUM_OUTPUTS };
	enum LightIds { NUM_LIGHTS };

	ABBox() {
		config(NUM_PARAMS, NUM_INPUTS, NUM_OUTPUTS, NUM_LIGHTS);

		configBiUpdate("PositionX", POS_X_PARAM, POS_X_INPUT, POS_X_ATTEN_PARAM,
				5.f);
		configBiUpdate("PositionY", POS_Y_PARAM, POS_Y_INPUT, POS_Y_ATTEN_PARAM,
				5.f);
		configBiUpdate("PositionZ", POS_Z_PARAM, POS_Z_INPUT, POS_Z_ATTEN_PARAM,
				5.f);

		configUpdate("ScaleX", SCALE_X_PARAM, SCALE_X_INPUT,
				SCALE_X_ATTEN_PARAM, 1.f);
		configUpdate("ScaleY", SCALE_Y_PARAM, SCALE_Y_INPUT,
				SCALE_Y_ATTEN_PARAM, 1.f);
		configUpdate("ScaleZ", SCALE_Z_PARAM, SCALE_Z_INPUT,
				SCALE_Z_ATTEN_PARAM, 1.f);
	}

	void update(const ProcessArgs& args) override {}
};

struct ABBoxWidget : URack::UModuleWidget {
	ABBoxWidget(ABBox* module) {
		setModule(module);
		setPanel(APP->window->loadSvg(
					asset::plugin(pluginInstance, "res/ABBox.svg")));

		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(16.92, 25.327)), module, ABBox::POS_X_ATTEN_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(29.035, 25.327)), module, ABBox::POS_X_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(16.983, 39.896)), module, ABBox::POS_Y_ATTEN_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(29.098, 39.896)), module, ABBox::POS_Y_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(17.083, 54.465)), module, ABBox::POS_Z_ATTEN_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(29.198, 54.465)), module, ABBox::POS_Z_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(17.083, 75.256)), module, ABBox::SCALE_X_ATTEN_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(29.198, 75.256)), module, ABBox::SCALE_X_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(17.145, 89.825)), module, ABBox::SCALE_Y_ATTEN_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(29.26, 89.825)), module, ABBox::SCALE_Y_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(17.245, 104.394)), module, ABBox::SCALE_Z_ATTEN_PARAM));
		addParam(createParamCentered<RoundBlackKnob>(
					mm2px(Vec(29.36, 104.394)), module, ABBox::SCALE_Z_PARAM));

		addInput(createInputCentered<PJ301MPort>(mm2px(Vec(7.17, 25.327)),
					module, ABBox::POS_X_INPUT));
		addInput(createInputCentered<PJ301MPort>(mm2px(Vec(7.233, 39.896)),
					module, ABBox::POS_Y_INPUT));
		addInput(createInputCentered<PJ301MPort>(mm2px(Vec(7.333, 54.465)),
					module, ABBox::POS_Z_INPUT));
		addInput(createInputCentered<PJ301MPort>(mm2px(Vec(7.333, 75.256)),
					module, ABBox::SCALE_X_INPUT));
		addInput(createInputCentered<PJ301MPort>(mm2px(Vec(7.396, 89.825)),
					module, ABBox::SCALE_Y_INPUT));
		addInput(createInputCentered<PJ301MPort>(mm2px(Vec(7.496, 104.394)),
					module, ABBox::SCALE_Z_INPUT));

		addPointCloudInput(mm2px(Vec(8.025, 120.652)), module,
				ABBox::POINT_CLOUD_INPUT, "PointCloud");

		addPointCloudOutput(mm2px(Vec(25.46, 120.652)), module,
				ABBox::POINTS_OUTSIDE_OUTPUT, "Outside");
		addPointCloudOutput(mm2px(Vec(39.368, 120.652)), module,
				ABBox::POINTS_INSIDE_OUTPUT, "Inside");
	}
};

Model* modelABBox = createModel<ABBox, ABBoxWidget>("ABBox");
