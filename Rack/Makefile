RACK_DIR = /home/matth/eidetic/Rack-SDK
URACK_DIR = /home/matth/eidetic/URack-VCV

# Include the URack components
include $(URACK_DIR)/plugin.mk

# Add .cpp files to the build
SOURCES += $(wildcard src/*.cpp)

# Add files to the ZIP package when running `make dist`
# The compiled plugin and "plugin.json" are automatically added.
DISTRIBUTABLES += res
DISTRIBUTABLES += $(wildcard LICENSE*)

# Include the Rack plugin Makefile framework
include $(RACK_DIR)/plugin.mk
