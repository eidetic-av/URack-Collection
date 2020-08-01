#include "plugin.hpp"

struct HighestPoint : URack::UModule {
	enum ParamIds { NUM_PARAMS };
	enum InputIds { POINTS_INPUT, NUM_INPUTS };
	enum OutputIds { HEIGHT_OUTPUT, POINTS_OUTPUT, NUM_OUTPUTS };
	enum LightIds { NUM_LIGHTS };

	HighestPoint() {
		config(NUM_PARAMS, NUM_INPUTS, NUM_OUTPUTS, NUM_LIGHTS);
		configListener("HEIGHT", HEIGHT_OUTPUT);
	}
};

struct HighestPointWidget : URack::UModuleWidget {
	HighestPointWidget(HighestPoint* module) {
		setModule(module);
		setPanel(APP->window->loadSvg(
					asset::plugin(pluginInstance, "res/HighestPoint.svg")));

		addPointCloudInput(mm2px(Vec(5.177, 50.713)), module,
				HighestPoint::POINTS_INPUT, "PointCloud");

		addOutput(createOutputCentered<PJ301MPort>(
					mm2px(Vec(5.585, 73.107)), module, HighestPoint::HEIGHT_OUTPUT));

		addPointCloudOutput(mm2px(Vec(4.869, 109.402)), module,
				HighestPoint::POINTS_OUTPUT, "PointCloudThru");
	}
};

Model* modelHighestPoint =
createModel<HighestPoint, HighestPointWidget>("HighestPoint");
