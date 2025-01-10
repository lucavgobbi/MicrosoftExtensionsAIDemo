using System.Numerics.Tensors;
using Microsoft.Extensions.AI;

namespace AIDemo;

public class OllamaRag
{
    private OllamaEmbeddingGenerator embeddingGenerator;

    private GeneratedEmbeddings<Embedding<float>>? embeddings;
    
    private static string[] faqs =
    [
            "What is your refund policy?: You can request a refund within 14 days of purchase, provided the book has not been downloaded or read for more than 10% of its content.",
    "How can I download a purchased book?: After purchase, the book will be available in your account's library for download or reading.",
    "What payment methods do you accept?: We accept credit/debit cards, PayPal, and bookstore gift cards.",
    "Do you offer discounts or sales?: Yes, we offer seasonal sales, discounts, and special promotions throughout the year.",
    "Can I pre-order upcoming books?: Yes, pre-orders are available for select upcoming book releases.",
    "Are there regional restrictions on books?: Some books may have regional restrictions based on publisher agreements. Please check the product page for details.",
    "How can I contact customer support?: You can contact customer support through our support page or via email at support@bookstore.com.",
    "What formats do you offer for books?: We offer eBooks, audiobooks, and printed books for select titles.",
    "How do I redeem a gift card or promo code?: You can redeem a gift card or promo code at checkout by entering the code in the designated field.",
    "Can I share my account with others?: Account sharing is against our terms of service and may result in account suspension.",
    "Do you offer book bundles?: Yes, we frequently offer bundles that include multiple books at discounted prices.",
    "What happens if a book I purchased is removed from the store?: If a book is removed, you will still have access to your purchased copy in your library.",
    "How do I verify my email address?: You will receive a verification link upon registration. Click the link to verify your email.",
    "Do you offer subscriptions for books?: Yes, we offer subscription services for access to a large selection of eBooks and audiobooks.",
    "Can I transfer my purchased books to another account?: No, purchases are tied to the account used during the transaction and cannot be transferred.",
    "How do I change my account password?: You can change your password in the account settings section.",
    "What should I do if my download is stuck?: Restart your device, ensure you have a stable internet connection, and try downloading again.",
    "Do you provide physical copies of books?: Yes, select titles are available in physical format, and we offer delivery options.",
    "Can I gift a book to someone?: Yes, you can purchase a book as a gift and send it to another user.",
    "What languages are supported on the platform?: We support books in multiple languages, including English, Spanish, French, German, and more.",
    "Is there a mobile app for your store?: Yes, our store is accessible via our mobile app available on iOS and Android.",
    "How do I track the books I've read or purchased?: Your account automatically tracks books you’ve purchased and allows you to mark books as read.",
    "What happens if my payment fails?: Check your payment details and try again. If the issue persists, contact your bank or our support team.",
    "Can I read books offline?: Yes, you can download eBooks or audiobooks to read or listen to offline.",
    "Do you support early access to books?: Yes, we provide access to select books before their official release date for pre-orders and subscribers.",
    "How do I update my billing information?: You can update your billing details in the payment methods section of your account settings.",
    "Do you have a loyalty program?: Yes, we offer a rewards program where you can earn points for purchases and redeem them for discounts.",
    "Can I cancel a pre-order?: Yes, pre-orders can be canceled up until the release date of the book.",
    "What security measures do you have to protect my data?: We use industry-standard encryption and security protocols to protect your personal information.",
    "How do I find personalized book recommendations?: Our platform offers personalized recommendations based on your reading history and preferences."
    ];

    public OllamaRag()
    {
        embeddingGenerator = new OllamaEmbeddingGenerator(
            new Uri("http://localhost:11434/"),
            modelId: "all-minilm"
        );
    }

    public async Task PopulateEmbeddingsAsync()
    {
        embeddings = await embeddingGenerator.GenerateAsync(faqs);
    }

    public async Task<string?> SearchEmbeddingsAsync(string query)
    {
        if (embeddings == null)
        {
            await PopulateEmbeddingsAsync();
        }

        var queryEmbedding = await embeddingGenerator.GenerateEmbeddingAsync(query);
        var bestMatchIndex = -1;
        var bestSimilarity = 0f;
        for (var i = 0; i < embeddings!.Count; i++)
        {
            var similarity = TensorPrimitives.CosineSimilarity(
                queryEmbedding.Vector.Span, 
                embeddings[i].Vector.Span);
            if (similarity > bestSimilarity)
            {
                bestSimilarity = similarity;
                bestMatchIndex = i;
            }
        }

        return bestMatchIndex > -1 ? faqs[bestMatchIndex] : null;
    }
}