namespace FinnanciaCSharp.DTOs.GenAI
{
    public class ApiResponse
    {
        public List<Candidate> candidates { get; set; } = new List<Candidate>();
    }
}

// {
//   "candidates": [
//     {
//       "content": {
//         "parts": [
//           {
//             "text": "O Santos Futebol Clube, também conhecido como Santos FC, é um clube de futebol brasileiro com sede na cidade de Santos, São Paulo. É um dos clubes mais tradicionais e bem-sucedidos do Brasil, tendo conquistado diversos títulos nacionais e internacionais, incluindo:\n\n**Títulos Nacionais:**\n\n* 8 Campeonatos Brasileiros (1961, 1962, 1963, 1964, 1965, 1968, 2002, 2004)\n* 4 Copas do Brasil (1961, 1962, 1963, 1964)\n* 1 Campeonato Brasileiro da Série B (2004)\n\n**Títulos Internacionais:**\n\n* 2 Copas Libertadores da América (1962, 1963)\n* 2 Copas Intercontinentais (1962, 1963)\n* 1 Copa Conmebol (1998)\n* 1 Recopa Sul-Americana (2012)\n* 1 Taça Rio Branco (1958)\n\n**Mas o Santos FC é muito mais do que seus títulos!**\n\n* O clube é conhecido por sua história rica e tradicional, sendo um dos primeiros clubes de futebol do Brasil.\n* Foi a casa de grandes lendas do futebol brasileiro, como Pelé, Neymar e Robinho.\n* Possui uma torcida apaixonada e fervorosa, conhecida como \"A Vila Belmiro\".\n* Se destaca por seus projetos sociais e pela formação de atletas, tendo revelado grandes jogadores para o futebol brasileiro e internacional.\n\n**O Santos FC é um símbolo do futebol brasileiro e uma fonte de orgulho para a cidade de Santos e seus torcedores.**\n"
//           }
//         ],
//         "role": "model"
//       },
//       "finishReason": "STOP",
//       "index": 0,
//       "safetyRatings": [
//         {
//           "category": "HARM_CATEGORY_SEXUALLY_EXPLICIT",
//           "probability": "NEGLIGIBLE"
//         },
//         {
//           "category": "HARM_CATEGORY_HATE_SPEECH",
//           "probability": "NEGLIGIBLE"
//         },
//         {
//           "category": "HARM_CATEGORY_HARASSMENT",
//           "probability": "NEGLIGIBLE"
//         },
//         {
//           "category": "HARM_CATEGORY_DANGEROUS_CONTENT",
//           "probability": "NEGLIGIBLE"
//         }
//       ]
//     }
//   ],
//   "usageMetadata": {
//     "promptTokenCount": 8,
//     "candidatesTokenCount": 390,
//     "totalTokenCount": 398
//   }
// }