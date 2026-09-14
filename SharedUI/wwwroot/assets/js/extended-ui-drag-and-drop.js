/**
 * Drag & Drop
 */
'use strict';

(function () {
  const cardEl = document.getElementById('sortable-cards'),
    pendingTasks = document.getElementById('pending-tasks'),
    completedTasks = document.getElementById('completed-tasks'),
    cloneSource1 = document.getElementById('clone-source-1'),
    cloneSource2 = document.getElementById('clone-source-2'),
    cloneSource3 = document.getElementById('clone-source-3'),
    cloneSource4 = document.getElementById('clone-source-4'),
    cloneSource5 = document.getElementById('clone-source-5'),
    cloneSource6 = document.getElementById('clone-source-6'),
    cloneSource7 = document.getElementById('clone-source-7'),
    cloneSource8 = document.getElementById('clone-source-8'),
    handleList1 = document.getElementById('handle-list-1'),
    handleList2 = document.getElementById('handle-list-2'),
    imageList1 = document.getElementById('image-list-1'),
    imageList2 = document.getElementById('image-list-2');

  // Cards
  // --------------------------------------------------------------------
  if (cardEl) {
    Sortable.create(cardEl);
  }

  // Images
  // --------------------------------------------------------------------
  if (imageList1) {
    Sortable.create(imageList1, {
      animation: 150,
      group: 'imgList'
    });
  }
  if (imageList2) {
    Sortable.create(imageList2, {
      animation: 150,
      group: 'imgList'
    });
  }

  // Cloning
  // --------------------------------------------------------------------
  if (cloneSource1) {
    Sortable.create(cloneSource1, {
      animation: 150,
      sort: false,
      group: {
        name: 'cloneList',
        pull: 'clone',
        revertClone: true,
        put: false
      }
    });
  }
  if (cloneSource2) {
    Sortable.create(cloneSource2, {
      animation: 150,
      group: {
        name: 'cloneList',
        pull: 'clone',
        revertClone: true
      }
    });
  }
  if (cloneSource3) {
    Sortable.create(cloneSource3, {
      animation: 150,
      group: {
        name: 'cloneList',
        pull: 'clone',
        revertClone: true
      }
    });
  }
  if (cloneSource4) {
    Sortable.create(cloneSource4, {
      animation: 150,
      group: {
        name: 'cloneList',
        pull: 'clone',
        revertClone: true
      }
    });
  }
  if (cloneSource5) {
    Sortable.create(cloneSource5, {
      animation: 150,
      group: {
        name: 'cloneList',
        pull: 'clone',
        revertClone: true
      }
    });
  }
  if (cloneSource6) {
    Sortable.create(cloneSource6, {
      animation: 150,
      group: {
        name: 'cloneList',
        pull: 'clone',
        revertClone: true
      }
    });
  }
  if (cloneSource7) {
    Sortable.create(cloneSource7, {
      animation: 150,
      group: {
        name: 'cloneList',
        pull: 'clone',
        revertClone: true
      }
    });
  }
  if (cloneSource8) {
    Sortable.create(cloneSource8, {
      animation: 150,
      group: {
        name: 'cloneList',
        pull: 'clone',
        revertClone: true
      }
    });
  }

  // Multiple
  // --------------------------------------------------------------------
  if (pendingTasks) {
    Sortable.create(pendingTasks, {
      animation: 150,
      group: 'taskList'
    });
  }
  if (completedTasks) {
    Sortable.create(completedTasks, {
      animation: 150,
      group: 'taskList'
    });
  }

  // Handles
  // --------------------------------------------------------------------
function initSortableLists() {
  document.querySelectorAll('.sortable-list').forEach((listElement, index) => {
    // إحذف الـ Sortable القديم لو موجود
    if (listElement._sortableInstance) {
      listElement._sortableInstance.destroy();
    }

    // إنشاء جديد
    const sortable = Sortable.create(listElement, {
      animation: 150,
      handle: '.drag-handle',
      onEnd: function (evt) {
        // طباعة الترتيب الجديد في الكونسول
        const order = [...listElement.children].map((item, i) =>
          item.dataset.id || `item-${i + 1}`
        );
        console.log(`🔁 [List ${index + 1}] New order:`, order);

        // تحديث عناوين الأسئلة حسب الترتيب الجديد
        updateQuestionNumbers(listElement);
      }
    });

    // نخزن النسخة في العنصر نفسه
    listElement._sortableInstance = sortable;
  });
}

// ✨ وظيفة لتحديث ترقيم العناوين داخل عنصر معين
function updateQuestionNumbers(listElement) {
  const items = listElement.querySelectorAll('li');
  items.forEach((item, i) => {
    const questionTitle = item.querySelector('p.mb-2');
    if (questionTitle) {
      questionTitle.textContent = `questions ${i + 1}`;
    }
  });
}





$(document).ready(function () {
  initSortableLists();

  // لما يتم إضافة عنصر جديد في أي ريبيتر
  $(document).on('click', '[data-repeater-create]', function () {
    setTimeout(() => {
      initSortableLists();
    }, 100); // علشان العنصر يظهر الأول
  });
});







  if (handleList2) {
    Sortable.create(handleList2, {
      animation: 150,
      group: 'handleList',
      handle: '.drag-handle'
    });
  }
})();
