function buildIISDomTree() {
    var data = [];
    $.ajax({
        url: '/GetAllIISInfos',
        type: 'GET',
        async: false,
        success: function (result) {
            console.log(result);
            data = result || [];
        }
    });
    return data;
}

function initializeIISTreeView() {
    var options = {
        bootstrap2: false,
        showTags: true,
        levels: 5,
        data: buildIISDomTree(),
    };
    $('#treeview').treeview(options);

    $('#context').contextmenu({
        target: '#context-menu',
        before: function (e, context) {
            // execute code before context menu if shown
            console.log(e, context);
            console.log(e.target.dataset.nodeid);
            var selectedNode = $('#treeview').treeview('getNode', e.target.dataset.nodeid);
            vm.cm.id = selectedNode.id;
            vm.cm.status = selectedNode.status;
            return selectedNode.hasContextMenu;
        },
        onItem: function (context, e) {
            var operation = e.target.dataset.operation;
            var handler = contextmenuHandlers[operation];
            if (handler != null) {
                handler(vm.cm.id, e);
            }
        }
    })
}

function execSiteCommand(cmd, id) {
    $.ajax({
        url: '/ExecSiteCommand',
        type: 'POST',
        data: { cmd: cmd, id: id },
        dataType: 'JSON',
        success: function (result) {
            if (result.success) {
                vm.cm.status = result.status;
            }
        }
    })
}

const contextmenuHandlers = {
    start: function (id, e) {
        execSiteCommand('start', id);
        e.preventDefault();
    },
    stop: function (id, e) {
        execSiteCommand('stop', id);
        e.preventDefault();
    }
}

var vm = new Vue({
    el: '#container',
    data: function () {
        return {
            title: 'IIS Manager Workbench',
            cm: {
                status: -1,
                name: ''
            }
        };
    },
    mounted: function () {
        initializeIISTreeView();
    },
    methods: {
        startSite: function (id, e) {

        },
        stopSite: function (id, e) {

        }
    }
})