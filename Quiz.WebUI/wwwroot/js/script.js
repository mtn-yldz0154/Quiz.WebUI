const quiz = new Quiz(sorular);
const ui = new UI();
let sayac;

ui.btn_start.addEventListener("click", function () {

    Sorusuresi = quiz.soruGetir().second;
    SoruPuani = quiz.soruGetir().puan;
    ui.quiz_box.classList.add("active");
    startTimer(Sorusuresi);
    startTimerLine(Sorusuresi);
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
    
    saniye = quiz.soruGetir().second - sayac;
    
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
  
    let bodyData = {
        token: token,
        skor: quiz.dogruCevapSayisi === 0 ? 0 : (SoruPuani - ((SoruPuani / Sorusuresi) * (Sorusuresi - sayac)))
    };

   

    fetch('/quiz/UpdateQuiz', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'X-Requested-With': 'XMLHttpRequest'
        },
        body: JSON.stringify(bodyData)

    })
        .then(response => {
            if (response.ok) {
                console.log("kaydedildi")
              
            } else {
                throw new Error("Quiz güncellenemedi!");
            }
        })
       
        .catch(error => {
            console.error("Error:", error);
        });
   
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

            if (quiz.soruGetir().questionType != 4) {
                ui.time_text.textContent = "Kalan Süre";


                let cevap = quiz.soruGetir().dogruCevap;

                for (let option of ui.option_list.children) {

                    if (option.querySelector("span b").textContent == cevap) {
                        option.classList.add("correct");
                        option.insertAdjacentHTML("beforeend", ui.correctIcon);
                    }

                    option.classList.add("disabled");
                }
            }

           
            document.getElementById("next_btn").click();
          
        }
    }
}

let counterLine;
function startTimerLine(timeInSeconds) {
    let line_width = 0;
    let maxWidth = 549; // Maksimum genişlik 549px
    let time_interval = 20; // Her 20ms'de bir çizgi genişliği güncellenecek
    let total_steps = timeInSeconds * 1000 / time_interval; // Toplam adım sayısı
    let increment = maxWidth / total_steps; // Her adımda genişliği ne kadar artıracağız

    counterLine = setInterval(timer, time_interval);

    function timer() {
        line_width += increment;
        ui.time_line.style.width = line_width + "px";

        if (line_width >= maxWidth) {
            clearInterval(counterLine);
        }
    }
}

