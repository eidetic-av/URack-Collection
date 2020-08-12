#include "plugin.hpp"

Plugin *pluginInstance;

extern "C" {
    URack::NetworkManager* baseManager = new URack::NetworkManager;
    URack::Settings* baseSettings = new URack::Settings;
    std::map<std::string, URack::UModule *>* moduleInstanceList = new std::map<std::string, URack::UModule *>();
    std::vector<int>* defaultHostSelections = new std::vector<int>();
}

void init(Plugin *p) {
    pluginInstance = p;

    // Add modules here
    p->addModel(modelDrone);
    p->addModel(modelHarmony);
    p->addModel(modelBillboard);
    p->addModel(modelGlowWorm);
    p->addModel(modelMirage2x);
    p->addModel(modelPlyPlayer);
    p->addModel(modelLiveScan3D);
    p->addModel(modelBounds);
    p->addModel(modelABBox);
    p->addModel(modelPointCounter);
    p->addModel(modelHighestPoint);
    p->addModel(modelInsider);
    p->addModel(modelDayTripper);
}
