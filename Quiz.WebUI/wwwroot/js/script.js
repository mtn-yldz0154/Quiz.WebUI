const quiz = new Quiz(sorular);
const ui = new UI();
let sayac;

ui.btn_start.addEventListener("click", function () {

    Sorusuresi = quiz.soruGetir().second;
    SoruPuani = quiz.soruGetir().puan;
    ui.quiz_box.classList.add("active");
    startTimer(Sorusuresi);
    startTimerLine();
    ui.soruGoster(quiz.soruGetir());
    ui.soruSayisiniGoster(quiz.soruIndex + 1, quiz.sorular.length);
    ui.btn_next.classList.remove("show");
    console.log(quiz.soruGetir().second)
});


ui.btn_replay.addEventListener("click", function() {
    quiz.soruIndex = 0;
    quiz.dogruCevapSayisi = 0;
    ui.btn_start.click();
    ui.score_box.classList.remove("active");
});


function optionSelected(option) {
  
    clearInterval(counterLine);
    GecenSure = counterLine;
    
    saniye = sayac;

    let cevap = option.querySelector("span b").textContent;
    let soru = quiz.soruGetir();

    if(soru.cevabiKontrolEt(cevap)) {
        quiz.dogruCevapSayisi += 1;
        option.classList.add("correct");
        option.insertAdjacentHTML("beforeend", ui.correctIcon);
    } else {
        option.classList.add("incorrect");
        option.insertAdjacentHTML("beforeend", ui.incorrectIcon);
    }

    for(let i=0; i < ui.option_list.children.length; i++) {
        ui.option_list.children[i].classList.add("disabled");
    }

    ui.btn_next.classList.add("show");
}

let counter;
let toplamZaman;

function startTimer(time) {
    counter = setInterval(timer, 1000);

    function timer() {
        sayac = time;

        ui.time_second.textContent = time;
        
        time--;
        
        if(time < 0) {
            clearInterval(counter);

            ui.time_text.textContent = "Kalan Süre";
            document.getElementById("next_btn").click();

            let cevap = quiz.soruGetir().dogruCevap;

            for(let option of ui.option_list.children) {

                if(option.querySelector("span b").textContent == cevap) {
                    option.classList.add("correct");
                    option.insertAdjacentHTML("beforeend", ui.correctIcon);
                }

                option.classList.add("disabled");
            }

            ui.btn_next.classList.add("show");
        }
    }
}

let counterLine;
function startTimerLine() {
    let line_width = 0;

    counterLine = setInterval(timer, 20);

    function timer() {
        line_width += 1;
        ui.time_line.style.width = line_width + "px";

        if(line_width > 549) {
            clearInterval(counterLine);
        }
    }
}


